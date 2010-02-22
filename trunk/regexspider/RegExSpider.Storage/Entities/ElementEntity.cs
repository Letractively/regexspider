using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegExSpider.Storage.Entities
{
    public class ElementEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } //the element name         
        public string Value { get; set; }
        public Dictionary<string, ElementEntity> Fields { get; set; } //Key: Field name, Value: Field data   

        public ElementEntity()
        {
            Fields = new Dictionary<string, ElementEntity>();
        }
    }
}
