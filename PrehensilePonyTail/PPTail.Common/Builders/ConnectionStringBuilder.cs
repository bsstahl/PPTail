using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PPTail.Builders
{
    public class ConnectionStringBuilder
    {
        const String _filePathKey = "Filepath";

        private readonly List<KeyValuePair<string, string>> _pairs = new List<KeyValuePair<string, string>>();

        String _providerName = string.Empty;

        public ConnectionStringBuilder(String providerName)
        {
            _providerName = providerName;
        }

        public String Build()
        {
            var sb = new StringBuilder();
            sb.Append($"Provider={_providerName};");
            foreach (var pair in _pairs)
            {
                sb.Append($"{pair.Key}={pair.Value};");
            }
            return sb.ToString();
        }

        public ConnectionStringBuilder ProviderName(String name)
        {
            _providerName = name;
            return this;
        }

        public ConnectionStringBuilder AddPair(String key, String value)
        {
            _pairs.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public ConnectionStringBuilder AddFilePath(String value)
        {
            return this.AddPair(_filePathKey, value);
        }
    }
}
