using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PPTail.Data.Forestry
{
    public static class StringExtensions
    {
        public static String ConditionalWrap(this string value, char? delimiter)
        {
            return delimiter.HasValue ? $"{delimiter}{value}{delimiter}" : value;
        }

        public static Boolean IsValidRecord(this string record, bool fuzzyMatch = false)
        {
            // A fuzzy match will allow any # of parts (split on colon)
            // as long as there are at least 2. The remaining parts are combined elsewhere.
            // 2 parts is a standard pair
            // 4 parts is TODO: Remember what this represents
            int partsCount = record.Split(':').Length;
            return fuzzyMatch ? partsCount > 1 : partsCount == 2 || partsCount == 4;
        }

        public static Guid ParseGuid(this string value)
        {
            Guid result = Guid.Empty;

            if (value.Length == 38)
                Guid.TryParse(value.Substring(1, 36), out result);
            else
                Guid.TryParse(value, out result);

            if (result == Guid.Empty)
                throw new InvalidOperationException($"Unable to parse a Guid from '{value}'");
                
            return result;
        }

        public static (String Key, String Value) ParseRecord(this string record)
        {
            (string, string) result = (string.Empty, string.Empty);
            var recordParts = record.Split(':');
            if (recordParts.Length == 2)
                result = (recordParts[0].Trim(), recordParts[1].Trim());
            else if (recordParts.Length > 1)
                result = (recordParts[0].Trim(), String.Join(":", recordParts.Skip(1).ToArray()).Trim());
            else
                throw new ArgumentException(nameof(record));

            return result;
        }

        public static IEnumerable<Category> ParseCategories(this String fileContents)
        {
            const string ID_KEY = "id";
            const string NAME_KEY = "name";
            const string DESCRIPTION_KEY = "description";

            var result = new List<Category>();

            if (!String.IsNullOrEmpty(fileContents))
            {
                var lines = fileContents.Split('\n');

                Category currentCategory = null;
                foreach (var line in lines)
                {
                    string field = string.Empty;
                    if (line.StartsWith("- "))
                    {
                        currentCategory = new Category();
                        result.Add(currentCategory);
                        field = line.Substring(2);
                    }
                    else if (!String.IsNullOrWhiteSpace(line))
                        field = line.Trim();

                    if (field.IsValidRecord())
                    {
                        var (key, value) = field.ParseRecord();
                        switch (key)
                        {
                            case ID_KEY:
                                currentCategory.Id = Guid.Parse(value);
                                break;

                            case NAME_KEY:
                                currentCategory.Name = value;
                                break;

                            case DESCRIPTION_KEY:
                                currentCategory.Description = value;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            return result;
        }

        public static Widget ParseWidget(this String fileContents)
        {
            const string ID_KEY = "id";
            const string TITLE_KEY = "title";
            const string SHOWTITLE_KEY = "showtitle";
            const string SHOWINSIDEBAR_KEY = "showinsidebar";
            const string ORDERINDEX_KEY = "orderindex";
            const string WIDGETTYPE_KEY = "widgettype";

            const string HR = "---";

            var result = new Widget();

            var fileSections = fileContents.Split(new[] { HR }, StringSplitOptions.RemoveEmptyEntries);
            int fieldCount = 0;

            if (fileSections.Length > 1)
            {
                result.Dictionary = new[] { new Tuple<string, string>("Content",
                    fileSections[1].Trim().ToHtml()) };
                fieldCount++;
            }

            if (fileSections.Length > 0)
            {
                var lines = fileContents.Split('\n');

                foreach (var line in lines)
                {
                    string field = String.IsNullOrWhiteSpace(line) 
                        ? string.Empty 
                        : line.Trim();

                    if (field.IsValidRecord(true))
                    {
                        var (key, value) = field.ParseRecord();
                        switch (key)
                        {
                            case ID_KEY:
                                result.Id = Guid.Parse(value);
                                break;

                            case TITLE_KEY:
                                result.Title = value;
                                break;

                            case WIDGETTYPE_KEY:
                                result.WidgetType = value.ParseWidgetType();
                                break;

                            case SHOWTITLE_KEY:
                                result.ShowTitle = Boolean.Parse(value);
                                break;

                            case SHOWINSIDEBAR_KEY:
                                result.ShowInSidebar = Boolean.Parse(value);
                                break;

                            case ORDERINDEX_KEY:
                                result.OrderIndex = Byte.Parse(value);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            return result;
        }

        public static WidgetType ParseWidgetType(this string value)
        {
            WidgetType result = WidgetType.Unknown;
            if (Enum.TryParse<WidgetType>(value, out var parseResult))
                result = parseResult;
            return result;
        }

        public static SiteSettings ParseSettings(this String fileContents)
        {
            const string POSTSPERPAGE_KEY = "postsperpage";
            const string POSTSPERFEED_KEY = "postsperfeed";
            const string TITLE_KEY = "title";
            const string THEME_KEY = "theme";
            const string DESCRIPTION_KEY = "description";
            const string COPYRIGHT_KEY = "copyright";
            const string CONTACTEMAIL_KEY = "contactemail";

            const string SITEVARIABLES_KEY = "sitevariables";
            const string SITEVARIABLENAME_KEY = "- variablename";
            const string SITEVARIABLEVALUE_KEY = "variablevalue";

            SiteSettings result = new SiteSettings();
            int settingsCount = 0;

            IEnumerable<(String Key, string Value)> lines = fileContents
                .Split('\n')
                .Where(l => l.IsValidRecord(true))
                .Select(l => l.ParseRecord());

            List<SiteVariable> siteVariables = null;
            SiteVariable currentSiteVariable = null;
            foreach (var line in lines)
            {
                switch (line.Key)
                {
                    case POSTSPERPAGE_KEY:
                        settingsCount++;
                        if (Int32.TryParse(line.Value, out var ppp))
                            result.PostsPerPage = ppp;
                        break;

                    case POSTSPERFEED_KEY:
                        settingsCount++;
                        if (Int32.TryParse(line.Value, out var ppf))
                            result.PostsPerFeed = ppf;
                        break;

                    case TITLE_KEY:
                        settingsCount++;
                        result.Title = line.Value;
                        break;

                    case DESCRIPTION_KEY:
                        settingsCount++;
                        result.Description = line.Value;
                        break;

                    case THEME_KEY:
                        settingsCount++;
                        result.Theme = line.Value;
                        break;

                    case COPYRIGHT_KEY:
                        settingsCount++;
                        result.Copyright = line.Value;
                        break;

                    case CONTACTEMAIL_KEY:
                        settingsCount++;
                        result.ContactEmail = line.Value;
                        break;

                    case SITEVARIABLES_KEY:
                        siteVariables = new List<SiteVariable>();
                        break;

                    case SITEVARIABLENAME_KEY:
                        currentSiteVariable = new SiteVariable { Name = line.Value.Trim() };
                        break;

                    case SITEVARIABLEVALUE_KEY:
                        if (currentSiteVariable.IsNotNull())
                        {
                            currentSiteVariable.Value = line.Value.Trim().Trim('"');
                            siteVariables.Add(currentSiteVariable);
                        }
                        break;

                    default:
                        break;
                }
            }

            if (siteVariables.IsNotNull())
                result.Variables = siteVariables;

            if (settingsCount == 0)
                throw new SettingNotFoundException(nameof(SiteSettings));

            return result;
        }

        public static ContentItem ParseContentItem(this String fileContents, IEnumerable<Category> categories)
        {
            const string TAGS_KEY = "tags";
            const string ID_KEY = "id";
            const string AUTHOR_KEY = "author";
            const string TITLE_KEY = "title";
            const string DESCRIPTION_KEY = "description";
            const string ISPUBLISHED_KEY = "ispublished";
            const string SHOWINLIST_KEY = "showinlist";
            const string PUBLICATIONDATE_KEY = "publicationdate";
            const string LASTMODIFICATIONDATE_KEY = "lastmodificationdate";
            const string SLUG_KEY = "slug";
            const string CATEGORIES_KEY = "categories";

            const string HR = "---";

            ContentItem result = new ContentItem
            {
                ByLine = string.Empty,
                Tags = new List<String>()
            };

            int fieldCount = 0;


            var fileSections = fileContents.Split(new[] { HR }, StringSplitOptions.RemoveEmptyEntries);

            if (fileSections.Length > 1)
            {
                string contentSection = String.Join($"\r\n{HR}\r\n", fileSections.Skip(1).Select(s => s.Trim()));
                result.Content = contentSection
                    .Trim()
                    .ToHtml();

                fieldCount++;
            }

            if (fileSections.Length > 0)
            {
                var frontMatter = fileSections[0].Trim().Split('\n');
                int[] keyIndexes = frontMatter
                    .Where(l => l.IsValidRecord(true))
                    .Select(l => Array.IndexOf(frontMatter, l))
                    .ToArray();

                var sections = new List<String[]>();

                // Add 1st n-1 sections
                for (int i = 0; i < keyIndexes.Length - 1; i++)
                    sections.Add(frontMatter
                        .ParseFrontMatterSection(keyIndexes[i], keyIndexes[i + 1])
                        .ToArray());

                // Add last section
                if (keyIndexes.Length > 0)
                    sections.Add(frontMatter
                        .ParseFrontMatterSection(keyIndexes[keyIndexes.Length - 1], frontMatter.Length)
                        .ToArray());

                foreach (var section in sections)
                {
                    if (section.Length > 1)
                    {
                        // Section is a collection type
                        var (key, value) = section.ParseFrontMatterCollection();

                        switch (key)
                        {
                            case TAGS_KEY:
                                fieldCount++;
                                result.Tags = value;
                                break;

                            case CATEGORIES_KEY:
                                fieldCount++;
                                result.CategoryIds = categories
                                    .Where(c => value.Any(v => v == c.Name))
                                    .Select(c => c.Id);
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        var line = section[0].ParseRecord();

                        switch (line.Key)
                        {
                            case ID_KEY:
                                fieldCount++;
                                result.Id = line.Value.ParseGuid();
                                break;

                            case AUTHOR_KEY:
                                fieldCount++;
                                result.Author = line.Value;
                                result.ByLine = String.IsNullOrWhiteSpace(line.Value) ? String.Empty : $"by {line.Value}";
                                break;

                            case TITLE_KEY:
                                fieldCount++;
                                result.Title = line.Value;
                                break;

                            case DESCRIPTION_KEY:
                                fieldCount++;
                                result.Description = line.Value;
                                break;

                            case ISPUBLISHED_KEY:
                                fieldCount++;
                                result.IsPublished = Boolean.Parse(line.Value);
                                break;

                            case SHOWINLIST_KEY:
                                fieldCount++;
                                result.ShowInList = Boolean.Parse(line.Value);
                                break;

                            case PUBLICATIONDATE_KEY:
                                fieldCount++;
                                result.PublicationDate = DateTime.Parse(line.Value);
                                break;

                            case LASTMODIFICATIONDATE_KEY:
                                fieldCount++;
                                result.LastModificationDate = DateTime.Parse(line.Value);
                                break;

                            case SLUG_KEY:
                                fieldCount++;
                                result.Slug = line.Value;
                                break;

                            default:
                                break;
                        }
                    }

                }
            }

            if (fieldCount == 0)
                result = null;

            return result;
        }

        public static IEnumerable<String> ParseFrontMatterSection(this String[] frontMatter, int startIndex, int endIndex)
        {
            var result = new List<String>();
            for (int i = startIndex; i < endIndex; i++)
                result.Add(frontMatter[i]);
            return result;
        }

        public static (string Key, IEnumerable<String> value) ParseFrontMatterCollection(this String[] frontMatter)
        {
            string key = frontMatter[0].Split(':')[0];

            var value = new List<String>();
            for (int i = 1; i < frontMatter.Length; i++)
            {
                if (frontMatter[i].Trim().StartsWith("- "))
                    value.Add(frontMatter[i].Trim().Substring(2));
            }

            return (key, value);
        }

        public static StringBuilder ConditionalAppendLine(this StringBuilder builder, bool addLine, String name, String value)
        {
            if (addLine)
                _ = string.IsNullOrEmpty(name)
                    ? builder.AppendLine(value)
                    : builder.AppendLine($"{name}: {value}");

            return builder;
        }

        public static StringBuilder ConditionalAppendList(this StringBuilder builder, bool addList, String name, IEnumerable<String> values)
        {
            if (addList)
            {
                if (values.IsNotNull() && values.Any())
                {
                    builder.AppendLine($"{name}:");
                    foreach (var value in values)
                        builder.AppendLine($"- {value}");
                }
                else
                    builder.AppendLine($"{name}: []");
            }
            return builder;
        }

        public static StringBuilder ConditionalAppendRepeatedField(this StringBuilder builder, bool addList, String collectionName, String fieldName, IEnumerable<String> values)
        {
            if (addList)
            {
                if (values.IsNotNull() && values.Any())
                {
                    builder.AppendLine($"{collectionName}:");
                    foreach (var value in values)
                        builder.AppendLine($"- {fieldName}: {value}");
                }
                else
                    builder.AppendLine($"{collectionName}: []");
            }
            return builder;
        }

        public static StringBuilder ConditionalAppendSiteVariables(this StringBuilder builder, bool addList, String collectionName, String fieldNameKey, String fieldValueKey, IEnumerable<SiteVariable> values)
        {
            if (addList)
            {
                if (values.IsNotNull() && values.Any())
                {
                    builder.AppendLine($"{collectionName}:");
                    foreach (var value in values)
                    {
                        builder.AppendLine($"{fieldNameKey}: {value.Name}");
                        builder.AppendLine($"{fieldValueKey}: \"{value.Value}\"");
                    }
                }
                else
                    builder.AppendLine($"{collectionName}: []");
            }
            return builder;
        }

        public static String ToMarkdown(this string html)
        {
            var config = new ReverseMarkdown.Config() { UnknownTags = ReverseMarkdown.Config.UnknownTagsOption.Bypass };
            var converter = new ReverseMarkdown.Converter(config);
            return converter.Convert(html).Replace("<br>", "\r\n");
        }

        public static String ToHtml(this string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown);
        }

        public static String Sanitize(this string value)
        {
            return value
                .Replace(" : ", " -- ")
                .Replace(" :", " -- ")
                .Replace(": ", " -- ")
                .Replace(":", "--");
        }

    }
}
