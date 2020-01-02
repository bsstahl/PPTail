using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public class ExtendedSettingsCollection : List<Tuple<String, String>>
    {
        public Tuple<String, String> Set(String name, String value)
        {
            var item = this.SingleOrDefault(t => t.Item1 == name);
            if (item != null)
            {
                _ = this.Remove(item);
            }

            item = new Tuple<String, String>(name, value);
            this.Add(item);

            return item;
        }

        public String Get(String name)
        {
            var setting = this.SingleOrDefault(s => s.Item1.ToUpperInvariant() == name.ToUpperInvariant());
            return (setting != null) ? setting.Item2 : String.Empty;
        }

        public Boolean HasSetting(String name) => this.Any(s => s.Item1.ToUpperInvariant() == name.ToUpperInvariant());
    }
}
