using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.FileSystem.Wordpress.Test
{
    public class PageJsonBuilder
    {
        int _id;
        DateTime _date;
        DateTime _lastModificationDate;
        string _guid;
        string _slug;
        string _status;
        string _type;
        string _title;
        string _content;
        string _excerpt;
        int _author;

        //Tags = tags,
        //ByLine = string.IsNullOrEmpty(author) ? string.Empty : $"by {author}",
        //CategoryIds = categoryIds

        public string Build()
        {
            return $"{{" +
                $"\"id\":\"{_id}\", " +
                $"\"author\":{_author}, " +
                $"\"slug\":\"{_slug}\", " +
                $"\"date_gmt\":\"{_date.ToString("s")}\", " +
                $"\"modified_gmt\":\"{_lastModificationDate.ToString("s")}\", " +
                $"\"title\": {{ \"rendered\" : \"{_title}\"}}, " +
                $"\"excerpt\": {{ \"rendered\" : \"{_excerpt}\"}}, " +
                $"\"content\": {{ \"rendered\" : \"{_content}\"}}, " +
                $"\"status\":\"{_status}\"" +
                $"}}";
        }

        public PageJsonBuilder AddRandomValues()
        {
            return this.UseId(9999.GetRandom())
                .UseAuthor(300.GetRandom())
                .UseTitle(string.Empty.GetRandom())
                .UseSlug(string.Empty.GetRandom())
                .UseStatus(string.Empty.GetRandom())
                .UseExcerpt(string.Empty.GetRandom())
                .UseContent(string.Empty.GetRandom())
                .UseDateGmt(DateTime.UtcNow.GetRandom())
                .UseModifiedDateGmt(DateTime.UtcNow.GetRandom());
        }

        public PageJsonBuilder UseId(int value)
        {
            _id = value;
            return this;
        }

        public PageJsonBuilder UseAuthor(int value)
        {
            _author = value;
            return this;
        }

        public PageJsonBuilder UseTitle(string value)
        {
            _title = value;
            return this;
        }

        public PageJsonBuilder UseSlug(string value)
        {
            _slug = value;
            return this;
        }

        public PageJsonBuilder UseStatus(string value)
        {
            _status = value;
            return this;
        }

        public PageJsonBuilder UseExcerpt(string value)
        {
            _excerpt = value;
            return this;
        }

        public PageJsonBuilder UseContent(string value)
        {
            _content = value;
            return this;
        }

        public PageJsonBuilder UseDateGmt(DateTime utcValue)
        {
            _date = utcValue;
            return this;
        }

        public PageJsonBuilder UseModifiedDateGmt(DateTime utcValue)
        {
            _lastModificationDate = utcValue;
            return this;
        }
    }
}
