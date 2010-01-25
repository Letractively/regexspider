using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegExSpider.Storage.Entities
{
    public class ElementStorageStatus
    {
        private int m_Stored;

        public int Stored
        {
            get { return m_Stored; }
            set { m_Stored = value; }
        }
        
    }
}
