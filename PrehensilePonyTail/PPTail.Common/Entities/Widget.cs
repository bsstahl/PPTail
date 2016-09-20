using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class Widget
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool ShowTitle { get; set; }
        public Enumerations.WidgetType WidgetType { get; set; }
        public IEnumerable<Tuple<string, string>> Dictionary { get; set; }
    }
}
