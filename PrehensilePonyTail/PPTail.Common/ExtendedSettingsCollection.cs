using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public class ExtendedSettingsCollection : List<Tuple<String, String>>
    {
        public Tuple<string, string> Set(string name, string value)
        {
            Tuple<string, string> item = this.SingleOrDefault(t => t.Item1 == name);
            if (item != null)
                this.Remove(item);

            item = new Tuple<string, string>(name, value);
            this.Add(item);

            return item;
        }

        public string Get(string name)
        {
            var setting = this.SingleOrDefault(s => s.Item1.ToLowerInvariant() == name.ToLowerInvariant());
            return (setting != null) ? setting.Item2 : string.Empty;
        }

        public bool HasSetting(string name)
        {
            return this.Any(s => s.Item1.ToLowerInvariant() == name.ToLowerInvariant());
        }
    }
}
