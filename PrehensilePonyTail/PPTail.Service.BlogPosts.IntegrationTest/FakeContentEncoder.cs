using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Service.BlogPosts.IntegrationTest
{
    public class FakeContentEncoder : IContentEncoder
    {
        public string HTMLEncode(string data)
        {
            return data;
        }

        public string UrlEncode(string data)
        {
            return data;
        }

        public string XmlEncode(string data)
        {
            return data;
        }
    }
}
