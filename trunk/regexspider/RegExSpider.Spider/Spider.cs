using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegExSpider.Configuration;
using RegExSpider.Storage;
using RegExSpider.Storage.Entities;
using System.Threading;

namespace RegExSpider.Spider
{
    public class Spider
    {
        public delegate void ReportStatus(ElementStorageStatus elements, LinkStorageStatus links);
        public event ReportStatus OnReportStatus = delegate { };

        private const int SCAN_CHUNK_SIZE = 50;

        private SiteEntity m_SiteEntity = null;
        private int m_CrawlersCapacity = 1; //default

        private ILinksStorage m_LinksStorage;
        private IElementStorage m_ElementStorage;

        private Thread m_WorkingThread;
        private int m_WorkingThreads = 0;

        private AutoResetEvent m_AutoResetEvent = new AutoResetEvent(false);

        private bool m_Paused = false;
        private AutoResetEvent m_PauseResetEvent = new AutoResetEvent(false);

        private object m_SyncObj = new object();

        public void Initialize(SiteEntity crawlSite, int crawlersCapacity)
        {
            //save the params
            m_SiteEntity = crawlSite;
            m_CrawlersCapacity = crawlersCapacity;

            //init the storage
            m_LinksStorage = new RegExSpider.Storage.XmlProvider.LinksStorage();
            m_ElementStorage = new RegExSpider.Storage.XmlProvider.ElementStorage();


            m_LinksStorage.InitializeStorage();
            m_ElementStorage.InitializeStorage();

            //load base crawl urls
            if (m_SiteEntity.StartPointUrls.Count > 0)
            {
                foreach (var item in m_SiteEntity.StartPointUrls)
                {
                    if (m_LinksStorage.IsExists(item) == false)
                        m_LinksStorage.InsertLink(new LinkEntity(0, item));
                }
            }
            else
            {
                if (m_LinksStorage.IsExists(m_SiteEntity.RootUrl) == false)
                    m_LinksStorage.InsertLink(new LinkEntity(0, m_SiteEntity.RootUrl));
            }
        }

        public void StartCrawling()
        {
            if (m_SiteEntity == null)
                throw new Exception("Spider needs to be initialized.");

            m_WorkingThread = new Thread(new ThreadStart(ScanSite));
            m_WorkingThread.Start();
        }

        public void StopCrawling()
        {
            m_WorkingThread.Abort();

            while (m_WorkingThreads > 0) { }

            m_LinksStorage.FinalizeStorage();
            m_ElementStorage.FinalizeStorage();
        }

        public void PauseCrawling()
        {
            m_Paused = true;
        }

        public void ResumeCrawling()
        {
            m_Paused = false;
            m_PauseResetEvent.Set();
        }

        void ScanSite()
        {
            IList<LinkEntity> linksQueue = m_LinksStorage.PoolWaitingUrls(SCAN_CHUNK_SIZE, m_SiteEntity.MaxDepth); ;

            do
            {
                if (m_WorkingThreads > 0 && linksQueue.Count == 0)
                {
                    m_AutoResetEvent.WaitOne();
                }

                if (linksQueue.Count > 0)
                {
                    foreach (var item in linksQueue)
                    {
                        if (m_WorkingThreads > m_CrawlersCapacity)
                        {
                            m_AutoResetEvent.WaitOne();
                        }

                        if (m_Paused)
                        {
                            m_PauseResetEvent.WaitOne();
                        }

                        PageHandler handler = new PageHandler(m_SiteEntity);
                        handler.OnHandlingFinished += new PageHandler.HandlingFinished(handler_OnHandlingFinished);
                        handler.OnFoundLinks += new PageHandler.FoundLinks(handler_OnFoundLinks);
                        handler.OnFoundElements += new PageHandler.FoundElements(handler_OnFoundElements);

                        Interlocked.Increment(ref m_WorkingThreads);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(handler.HandlePage), item);
                    }
                }

                linksQueue = m_LinksStorage.PoolWaitingUrls(SCAN_CHUNK_SIZE, m_SiteEntity.MaxDepth);

            } while (linksQueue.Count > 0 || m_WorkingThreads > 0);

            m_LinksStorage.FinalizeStorage();
            m_ElementStorage.FinalizeStorage();
        }

        void handler_OnFoundElements(List<ElementEntity> elements)
        {
            foreach (var item in elements)
            {
                m_ElementStorage.InsertElement(item);
            }
        }

        void handler_OnFoundLinks(List<string> links, int handlerDepth)
        {
            foreach (var item in links)
            {
                if (m_LinksStorage.IsExists(item) == false)
                    m_LinksStorage.InsertLink(new LinkEntity(handlerDepth + 1, item));
            }
        }

        void handler_OnHandlingFinished(LinkEntity link)
        {
            Interlocked.Decrement(ref m_WorkingThreads);
            m_LinksStorage.LinkScanned(link);
            m_AutoResetEvent.Set();

            OnReportStatus(m_ElementStorage.GetStatus(), m_LinksStorage.GetStatus());
        }
    }
}
