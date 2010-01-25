using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegExSpider.Storage.Entities;

namespace RegExSpider.Storage
{
    public interface ILinksStorage
    {
        //initialize the links storage
        void InitializeStorage();
        void FinalizeStorage();
        //insert a new link to the scan queue.
        void InsertLink(LinkEntity link);
        void InsertLinks(IList<LinkEntity> links);
        //check to see if a specific url already exists (scanned or not...)
        bool IsExists(string url);
        //pools a list of LinkEntities waiting to be scanned.
        IList<LinkEntity> PoolWaitingUrls(int size, int maxDepth);
        //called when a link has been scanned.
        void LinkScanned(LinkEntity link);
        //Returns a storage status report
        LinkStorageStatus GetStatus();
    }
}
