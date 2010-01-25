using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegExSpider.Storage.Entities;

namespace RegExSpider.Configuration
{
    public class SiteEntity
    {
        public string Name { get; set; }
        public string RootUrl { get; set; }
        public int MaxDepth { get; set; }

        public List<string> StartPointUrls { get; set; }
        public List<string> NoFollowExpressions { get; set; }
        public List<ExtractionElement> ExtractionElements { get; set; }

        public SiteEntity()
        {
            StartPointUrls = new List<string>();
            NoFollowExpressions = new List<string>();
            ExtractionElements = new List<ExtractionElement>();
        }

    }
}
