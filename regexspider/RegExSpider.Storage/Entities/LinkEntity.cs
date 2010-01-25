using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegExSpider.Storage.Entities
{
    public class LinkEntity
    {
        public int Id { get; set; }
        public int Depth { get; set; }
        public string Url { get; set; }

        public LinkEntity(int depth, string url)
        {    
            this.Url = url;
            this.Depth = depth;
        }
    }
}
