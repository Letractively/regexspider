using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegExSpider.Storage.Entities
{
    public class LinkStorageStatus
    {
        private int m_Waiting;       
        private int m_Scanned;

        public int Waiting
        {
            get { return m_Waiting; }
            set { m_Waiting = value; }
        }
        public int Scanned
        {
            get { return m_Scanned; }
            set { m_Scanned = value; }
        }
        
    }
}
