using PPTail.Exceptions;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Linq;

namespace PPTail.Web.PostLocator
{
    public class CachingProvider: Interfaces.IPostLocator
    {
        Dictionary<Guid, string> _posts;
        IServiceProvider _serviceProvider;
        bool _loaded = false;

        public CachingProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<IFile>();
            _serviceProvider.ValidateService<IHostingEnvironment>();
        }

        private void Load()
        {
            var env = _serviceProvider.GetService<IHostingEnvironment>();
            string dataFilePath = $"{env.WebRootPath}\\app_data\\posts.xml";

            var file = _serviceProvider.GetService<IFile>();
            var postXml = file.ReadAllText(dataFilePath);
            var xml = XElement.Parse(postXml);
            var postNodes = xml.Descendants();

            _posts = new Dictionary<Guid, string>();
            foreach (var postNode in postNodes)
            {
                string idString = postNode.Attributes().Single(a => a.Name.LocalName == "id").Value;
                string url = postNode.Attributes().Single(a => a.Name.LocalName == "url").Value;
                _posts.Add(Guid.Parse(idString), url);
            }

            _loaded = true;
        }

        public string GetUrlByPostId(Guid id)
        {
            if (!_loaded)
                Load();

            Func<KeyValuePair<Guid, string>, bool> predicate = p => p.Key == id;
            if (_posts.Any(predicate))
                return _posts.Single(predicate).Value;
            else
                throw new PostNotFoundException(id);
        }
    }
}
