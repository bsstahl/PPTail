using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PPTail.Common.Builders
{
    public class ConnectionStringBuilder
    {
        const string _filePathKey = "Filepath";

        private readonly List<KeyValuePair<string, string>> _pairs = new List<KeyValuePair<string, string>>();

        string _providerName = string.Empty;

        public ConnectionStringBuilder(string providerName)
        {
            _providerName = providerName;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            sb.Append($"Provider={_providerName};");
            foreach (var pair in _pairs)
            {
                sb.Append($"{pair.Key}={pair.Value};");
            }
            return sb.ToString();
        }

        public ConnectionStringBuilder ProviderName(string name)
        {
            _providerName = name;
            return this;
        }

        public ConnectionStringBuilder AddPair(string key, string value)
        {
            _pairs.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public ConnectionStringBuilder AddFilePath(string value)
        {
            return this.AddPair(_filePathKey, value);
        }
    }
}
