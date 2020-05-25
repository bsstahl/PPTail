using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.Forestry.Test
{
    internal class ContentItemFileBuilder
    {
        private readonly ContentItem _item = new ContentItem();

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
        private bool _removeCategoryIds = false;
        private bool _removeContent = false;

        public String Build()
        {
            var node = new StringBuilder();
            return node.AppendLine("---")
                .ConditionalAppendList(!_removeTags, "tags", _item.Tags)
                .ConditionalAppendLine(!_removeMenuOrder, "menuorder", _item.MenuOrder.ToString())
                .ConditionalAppendLine(!_removeId, "id", _item.Id.ToString())
                .ConditionalAppendLine(!_removeAuthor, "author", _item.Author)
                .ConditionalAppendLine(!_removeTitle, "title", _item.Title)
                .ConditionalAppendLine(!_removeDescription, "description", _item.Description)
                .ConditionalAppendLine(!_removeIsPublished, "ispublished", _item.IsPublished.ToString().ToLower())
                .ConditionalAppendLine(!_removeShowInList, "showinlist", _item.ShowInList.ToString().ToLower())
                .ConditionalAppendLine(!_removePublicationDate, "publicationdate", _item.PublicationDate.ToString("s"))
                .ConditionalAppendLine(!_removeLastModificationDate, "lastmodificationdate", _item.LastModificationDate.ToString("s"))
                .ConditionalAppendLine(!_removeSlug, "slug", _item.Slug)
                .ConditionalAppendList(!_removeCategoryIds, "categoryids", _item.CategoryIds?.Select(i => i.ToString()))
                .AppendLine("")
                .AppendLine("---")
                .ConditionalAppendLine(!_removeContent, string.Empty, _item.Content)
                .ToString();
        }

        public ContentItemFileBuilder UseRandomValues()
        {
            return this
                .Tags(new[] { string.Empty.GetRandom() })
                .Id(Guid.NewGuid())
                .Author(string.Empty.GetRandom(10))
                .Title(string.Empty.GetRandom(15))
                .Description(string.Empty.GetRandom(25))
                .IsPublished(true)
                .ShowInList(true)
                .PublicationDate(DateTime.Parse("1/1/2000").AddSeconds(Int32.MaxValue))
                .LastModificationDate(DateTime.Parse("1/1/2000").AddSeconds(Int32.MaxValue))
                .Slug(string.Empty.GetRandom(20))
                .CategoryIds(new[] { Guid.NewGuid() })
                .Content(string.Empty.GetRandom(200));
        }

        public ContentItemFileBuilder Tags(IEnumerable<String> value)
        {
            _item.Tags = value;
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
            _item.Id = value;
            _removeId = false;
            return this;
        }

        public ContentItemFileBuilder RemoveId()
        {
            _removeId = true;
            return this;
        }

        public ContentItemFileBuilder Author(String value)
        {
            _item.Author = value;
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
            _item.Title = value;
            _removeTitle = false;
            return this;
        }

        public ContentItemFileBuilder RemoveTitle()
        {
            _removeTitle = true;
            return this;
        }

        public  ContentItemFileBuilder Description(String value)
        {
            _item.Description = value;
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
            _item.IsPublished = value;
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
            _item.ShowInList = value;
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
            _item.PublicationDate = value;
            _removePublicationDate = false;
            return this;
        }

        public ContentItemFileBuilder RemovePublicationDate()
        {
            _removePublicationDate = true;
            return this;
        }

        public ContentItemFileBuilder LastModificationDate(DateTime value)
        {
            _item.LastModificationDate = value;
            _removeLastModificationDate = false;
            return this;
        }

        public ContentItemFileBuilder RemoveLastModificationDate()
        {
            _removeLastModificationDate = true;
            return this;
        }

        public ContentItemFileBuilder Slug(String value)
        {
            _item.Slug = value;
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
            _item.Content = value;
            _removeContent = false;
            return this;
        }

        public ContentItemFileBuilder RemoveContent()
        {
            _removeContent = true;
            return this;
        }

        public ContentItemFileBuilder CategoryIds(IEnumerable<Guid> value)
        {
            _item.CategoryIds = value;
            _removeCategoryIds = false;
            return this;
        }

        public ContentItemFileBuilder RemoveCategoryIds()
        {
            _removeCategoryIds = true;
            return this;
        }

    }
}
