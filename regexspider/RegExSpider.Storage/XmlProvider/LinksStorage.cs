using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RegExSpider.Storage.Entities;

namespace RegExSpider.Storage.XmlProvider
{
    public class LinksStorage:ILinksStorage
    {
        private XmlTextWriter m_XmlTextWriter;
        private List<LinkEntity> m_WaitingLinks;
        private List<LinkEntity> m_ScannedLinks;

        private object m_SyncWaitingList = new object();
        private object m_SyncScannedList = new object();
        private object m_SyncXmlWriter = new object();

        #region ILinksStorage Members

        public void InitializeStorage()
        {
            m_WaitingLinks = new List<LinkEntity>();
            m_ScannedLinks = new List<LinkEntity>();

            m_XmlTextWriter = new XmlTextWriter("links.xml", Encoding.UTF8);
            
            m_XmlTextWriter.Formatting = Formatting.Indented;
            m_XmlTextWriter.Indentation = 1;
            m_XmlTextWriter.IndentChar =  ' ';

            m_XmlTextWriter.WriteStartDocument();
            m_XmlTextWriter.WriteStartElement("Links");
        }

        public void FinalizeStorage()
        {
            lock (m_SyncXmlWriter)
            {
                m_XmlTextWriter.WriteEndElement();
                m_XmlTextWriter.WriteEndDocument();
                m_XmlTextWriter.Close(); 
            }
        }

        public void InsertLink(RegExSpider.Storage.Entities.LinkEntity link)
        {
            lock (m_SyncWaitingList)
            {
                m_WaitingLinks.Add(link); 
            }

            lock (m_SyncXmlWriter)
            {
                m_XmlTextWriter.WriteStartElement("Link");
                m_XmlTextWriter.WriteString(link.Url);
                m_XmlTextWriter.WriteEndElement();       
            }      
        }

        public void InsertLinks(IList<RegExSpider.Storage.Entities.LinkEntity> links)
        {
            foreach (var item in links)
            {
                InsertLink(item);
            }
        }

        public bool IsExists(string url)
        {
            bool exists = false;

            lock (m_SyncWaitingList)
            {
                exists |= m_WaitingLinks.Exists(o => o.Url.Equals(url)); 
            }
            lock (m_ScannedLinks)
            {
                exists |= m_ScannedLinks.Exists(o => o.Url.Equals(url)); 
            }

            return exists;
        }

        public IList<LinkEntity> PoolWaitingUrls(int size, int maxDepth)
        {
            List<LinkEntity> pool = new List<LinkEntity>();
            lock (m_SyncWaitingList)
            {
                pool = (List<LinkEntity>)m_WaitingLinks.FindAll(o => o.Depth <= maxDepth).Take(size).ToList();

                foreach (var item in pool)
                    m_WaitingLinks.Remove(item); 
            }
            
            return pool;
        }

        public void LinkScanned(LinkEntity link)
        {
            lock (m_SyncWaitingList)
            {
                m_WaitingLinks.RemoveAll(o => o.Url.Equals(link.Url)); 
            }
            lock (m_ScannedLinks)
            {
                m_ScannedLinks.Add(link); 
            }
        }

        public LinkStorageStatus GetStatus()
        {
            LinkStorageStatus status = new LinkStorageStatus();
            status.Scanned = m_ScannedLinks.Count;
            status.Waiting = m_WaitingLinks.Count;
            return status;
        }

        #endregion
    }
}
