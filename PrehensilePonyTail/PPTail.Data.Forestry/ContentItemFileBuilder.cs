using PPTail.Entities;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Data.Forestry
{
    public class ContentItemFileBuilder
    {
        private Guid _id;
        private String _author;
        private String _title;
        private String _description;
        private String _content;
        private bool _isPublished;
        private bool _showInList;
        private DateTime _publicationDate;
        private DateTime _lastModificationDate;
        private String _slug;
        private IEnumerable<string> _tags;
        private IEnumerable<String> _categories;
        private Int32 _menuOrder;

        private String _publicationDateSerializationFormat = "s";
        private String _lastModificationDateSerializationFormat = "s";
        private char? _idDelimiter = null;

        private bool _removeTags = false;
        private bool _removeMenuOrder = false;
        private bool _removeId = false;
        private bool _removeAuthor = false;
        private bool _removeTitle = false;
        private bool _removeDescription = false;
        private bool _removeIsPublished = false;
        private bool _removeShowInList = false;
        private bool _removePublicationDate = false;
        private bool _removeLastModificationDate = false;
        private bool _removeSlug = false;
        private bool _removeCategories = false;
        private bool _removeContent = false;


        public ContentItemFileBuilder() { }

        public ContentItemFileBuilder(ContentItem item, IEnumerable<Category> categories)
        {
            this.Id(item.Id);
            this.Author(item.Author);
            this.Title(item.Title);
            this.Description(item.Description);
            this.Content(item.Content);
            this.IsPublished(item.IsPublished);
            this.ShowInList(item.ShowInList);
            this.PublicationDate(item.PublicationDate);
            this.LastModificationDate(item.LastModificationDate);
            this.Slug(item.Slug);
            this.Tags(item.Tags);
            this.CategoryIds(item.CategoryIds, categories);
            this.MenuOrder(item.MenuOrder);
        }

        public String Build()
        {
            var node = new StringBuilder();
            return node.AppendLine("---")
                .ConditionalAppendList(!_removeTags, "tags", _tags)
                .ConditionalAppendLine(!_removeMenuOrder, "menuorder", _menuOrder.ToString())
                .ConditionalAppendLine(!_removeId, "id", _id.ToString().ConditionalWrap(_idDelimiter))
                .ConditionalAppendLine(!_removeAuthor, "author", _author.Sanitize())
                .ConditionalAppendLine(!_removeTitle, "title", _title.Sanitize())
                .ConditionalAppendLine(!_removeDescription, "description", _description.Sanitize())
                .ConditionalAppendLine(!_removeIsPublished, "ispublished", _isPublished.ToString().ToLower())
                .ConditionalAppendLine(!_removeShowInList, "showinlist", _showInList.ToString().ToLower())
                .ConditionalAppendLine(!_removePublicationDate, "publicationdate", _publicationDate.ToString(_publicationDateSerializationFormat))
                .ConditionalAppendLine(!_removeLastModificationDate, "lastmodificationdate", _lastModificationDate.ToString(_lastModificationDateSerializationFormat))
                .ConditionalAppendLine(!_removeSlug, "slug", _slug)
                .ConditionalAppendList(!_removeCategories, "categories", _categories)
                .AppendLine("")
                .AppendLine("---")
                .ConditionalAppendLine(!_removeContent, string.Empty, _content.ToMarkdown())
                .ToString();
        }

        public ContentItemFileBuilder Tags(IEnumerable<String> value)
        {
            _tags = value;
            _removeTags = false;
            return this;
        }

        public ContentItemFileBuilder RemoveTags()
        {
            _removeTags = true;
            return this;
        }

        public ContentItemFileBuilder Id(Guid value)
        {
            _id = value;
            _removeId = false;
            return this;
        }

        public ContentItemFileBuilder IdDelimiter(char? value)
        {
            _idDelimiter = value;
            return this;
        }

        public ContentItemFileBuilder RemoveId()
        {
            _removeId = true;
            return this;
        }

        public ContentItemFileBuilder Author(String value)
        {
            _author = value;
            _removeAuthor = false;
            return this;
        }

        public ContentItemFileBuilder RemoveAuthor()
        {
            _removeAuthor = true;
            return this;
        }

        public ContentItemFileBuilder Title(String value)
        {
            _title = value;
            _removeTitle = false;
            return this;
        }

        public ContentItemFileBuilder RemoveTitle()
        {
            _removeTitle = true;
            return this;
        }

        public ContentItemFileBuilder Description(String value)
        {
            _description = value;
            _removeDescription = false;
            return this;
        }

        public ContentItemFileBuilder RemoveDescription()
        {
            _removeDescription = true;
            return this;
        }

        public ContentItemFileBuilder IsPublished(Boolean value)
        {
            _isPublished = value;
            _removeIsPublished = false;
            return this;
        }

        public ContentItemFileBuilder RemoveIsPublished()
        {
            _removeIsPublished = true;
            return this;
        }

        public ContentItemFileBuilder ShowInList(Boolean value)
        {
            _showInList = value;
            _removeShowInList = false;
            return this;
        }

        public ContentItemFileBuilder RemoveShowInList()
        {
            _removeShowInList = true;
            return this;
        }

        public ContentItemFileBuilder PublicationDate(DateTime value)
        {
            _publicationDate = value;
            _removePublicationDate = false;
            return this;
        }

        public ContentItemFileBuilder PublicationDateSerializationFormat(String value)
        {
            _publicationDateSerializationFormat = value;
            return this;
        }

        public ContentItemFileBuilder RemovePublicationDate()
        {
            _removePublicationDate = true;
            return this;
        }

        public ContentItemFileBuilder LastModificationDate(DateTime value)
        {
            _lastModificationDate = value;
            _removeLastModificationDate = false;
            return this;
        }

        public ContentItemFileBuilder LastModificationDateSerializationFormat(String value)
        {
            _lastModificationDateSerializationFormat = value;
            return this;
        }

        public ContentItemFileBuilder RemoveLastModificationDate()
        {
            _removeLastModificationDate = true;
            return this;
        }

        public ContentItemFileBuilder Slug(String value)
        {
            _slug = value;
            _removeSlug = false;
            return this;
        }

        public ContentItemFileBuilder RemoveSlug()
        {
            _removeSlug = true;
            return this;
        }


        public ContentItemFileBuilder Content(String value)
        {
            _content = value;
            _removeContent = false;
            return this;
        }

        public ContentItemFileBuilder RemoveContent()
        {
            _removeContent = true;
            return this;
        }

        public ContentItemFileBuilder Categories(IEnumerable<String> value)
        {
            _categories = value;
            _removeCategories = false;
            return this;
        }

        public ContentItemFileBuilder CategoryIds(IEnumerable<Guid> value, IEnumerable<Category> categories)
        {
            var categoryNames = new List<String>();
            foreach (var categoryId in value)
            {
                var category = categories.SingleOrDefault(c => c.Id == categoryId);
                if (category.IsNotNull())
                    categoryNames.Add(category.Name);
            }

            return this.Categories(categoryNames);
        }

        public ContentItemFileBuilder RemoveCategories()
        {
            _removeCategories = true;
            return this;
        }

        public ContentItemFileBuilder MenuOrder(int value)
        {
            _menuOrder = value;
            _removeMenuOrder = false;
            return this;
        }

        public ContentItemFileBuilder RemoveMenuOrder()
        {
            _removeMenuOrder = true;
            return this;
        }

    }
}
