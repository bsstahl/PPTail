using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PPTail.Data.Forestry
{
    public static class StringExtensions
    {
        public static Boolean IsValidRecord(this string record)
        {
            int partsCount = record.Split(':').Length;
            return (partsCount == 2 || partsCount == 4);
        }

        public static (String Key, String Value) ParseRecord(this string record)
        {
            (string, string) result = (string.Empty, string.Empty);
            var recordParts = record.Split(':');
            if (recordParts.Length == 2)
                result = (recordParts[0].Trim(), recordParts[1].Trim());
            else if (recordParts.Length == 4)
                result = (recordParts[0].Trim(), String.Join(":", new[] { recordParts[1], recordParts[2], recordParts[3] }));
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

            return result;
        }

        public static IEnumerable<Widget> ParseWidgets(this String fileContents)
        {
            const string ID_KEY = "id";
            const string TITLE_KEY = "title";
            const string SHOWTITLE_KEY = "showtitle";
            const string WIDGETTYPE_KEY = "widgettype";
            const string DICTIONARY_KEY = "dictionary";
            const string DICTIONARYKEY_KEY = "- key";
            const string DICTIONARYVALUE_KEY = "value";

            var result = new List<Widget>();
            var lines = fileContents.Split('\n');

            Widget currentWidget = null;
            List<Tuple<string, string>> currentDictionary = null;
            Tuple<string, string> currentDictionaryItem = null;

            foreach (var line in lines)
            {
                string field = string.Empty;
                if (line.StartsWith("- "))
                {
                    currentWidget = new Widget();
                    result.Add(currentWidget);
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
                            currentWidget.Id = Guid.Parse(value);
                            currentDictionary = null;
                            currentDictionaryItem = null;
                            break;

                        case TITLE_KEY:
                            currentWidget.Title = value;
                            currentDictionary = null;
                            currentDictionaryItem = null;
                            break;

                        case WIDGETTYPE_KEY:
                            currentWidget.WidgetType = value.ParseWidgetType();
                            currentDictionary = null;
                            currentDictionaryItem = null;
                            break;

                        case SHOWTITLE_KEY:
                            currentWidget.ShowTitle = Boolean.Parse(value);
                            currentDictionary = null;
                            currentDictionaryItem = null;
                            break;

                        case DICTIONARY_KEY:
                            currentDictionary = new List<Tuple<string, string>>();
                            currentWidget.Dictionary = currentDictionary;
                            if (value.Replace(" ", "") == "[]")
                                currentDictionary = null;
                            break;

                        case DICTIONARYKEY_KEY:
                            if (currentDictionary.IsNotNull())
                                currentDictionaryItem = new Tuple<string, string>(value, string.Empty);
                            break;

                        case DICTIONARYVALUE_KEY:
                            if (currentDictionaryItem.IsNotNull())
                            {
                                currentDictionaryItem = new Tuple<string, string>(currentDictionaryItem.Item1, value);
                                currentDictionary.Add(currentDictionaryItem);
                                currentDictionaryItem = null;
                            }  
                            break;

                        default:
                            break;
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

            SiteSettings result = new SiteSettings();
            int settingsCount = 0;

            IEnumerable<(String Key, string Value)> lines = fileContents
                .Split('\n')
                .Where(l => l.IsValidRecord())
                .Select(l => l.ParseRecord());

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

                    default:
                        break;
                }
            }

            if (settingsCount == 0)
                throw new SettingNotFoundException(nameof(SiteSettings));

            return result;
        }

        public static ContentItem ParseContentItem(this String fileContents)
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
            const string CATEGORYIDS_KEY = "categoryids";

            ContentItem result = new ContentItem();
            int fieldCount = 0;

            result.ByLine = string.Empty;

            var fileSections = fileContents.Split(new[] { "---" }, StringSplitOptions.RemoveEmptyEntries);

            if (fileSections.Length > 1)
            {
                result.Content = fileSections[1].Trim();
                fieldCount++;
            }
            
            if (fileSections.Length > 0)
            {
                var frontMatter = fileSections[0].Trim().Split('\n');
                int[] keyIndexes = frontMatter
                    .Where(l => l.IsValidRecord())
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

                            case CATEGORYIDS_KEY:
                                fieldCount++;
                                result.CategoryIds = value.Select(v => Guid.Parse(v));
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
                                result.Id = Guid.Parse(line.Value);
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

    }
}
