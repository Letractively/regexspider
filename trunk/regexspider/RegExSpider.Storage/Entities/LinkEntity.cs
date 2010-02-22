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

        public LinkEntity(int id, int depth, string url)
        {
            this.Id = id;
            this.Url = url;
            this.Depth = depth;
        }
    }
}
