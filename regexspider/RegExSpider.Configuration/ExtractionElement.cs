using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegExSpider.Configuration
{
    public class ExtractionElement
    {
        public string Name { get; set; }
        public string RegEx { get; set; }

        public Dictionary<string, ExtractionElement> Fields { get; set; }

        public ExtractionElement()
        {
            Fields = new Dictionary<string, ExtractionElement>();
        }
    }
}
