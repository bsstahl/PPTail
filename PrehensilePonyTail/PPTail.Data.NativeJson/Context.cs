using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.NativeJson
{
    public class Context
    {
        public IEnumerable<Entities.ContentItem> Pages { get; set; }
        public IEnumerable<Entities.ContentItem> Posts { get; set; }
        public IEnumerable<Entities.Widget> Widgets { get; set; }
        public IEnumerable<Entities.Category> Categories { get; set; }
        public Entities.SiteSettings SiteSettings { get; set; }

        internal void Save(String filePath)
        {
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            System.IO.File.WriteAllText(filePath, jsonData);
        }

        internal static Context Load(String filePath)
        {
            var jsonData = System.IO.File.ReadAllText(filePath);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Context>(jsonData);
        }
    }
}
