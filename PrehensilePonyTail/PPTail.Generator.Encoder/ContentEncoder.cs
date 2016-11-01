using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Encoder
{
    public class ContentEncoder : IContentEncoder
    {
        public ContentEncoder(IServiceProvider serviceProvider)
        {
        }

        public string HTMLEncode(string data)
        {
            return data.Replace("&quot;", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace("&lt;", "")
                .Replace(">", "")
                .Replace("&gt;", "")
                .Replace("!", "bang")
                .Replace("“", "")
                .Replace("”", "")
                .Replace("–", "-")
                .Replace(".", "dot")
                .Replace("e28093", "-")
                .Replace("e2809c", "")
                .Replace("e2809d", "");
        }

        public string UrlEncode(string data)
        {
            return HTMLEncode(data.Trim().Replace(' ', '-')).RemoveConsecutiveDashes();
        }

        public string XmlEncode(string data)
        {
            return data.Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("“", "&quot;")
                .Replace("”", "&quot;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }


    }
}
