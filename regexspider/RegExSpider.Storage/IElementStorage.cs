using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegExSpider.Storage.Entities;

namespace RegExSpider.Storage
{
    public interface IElementStorage
    {
        //initialize the element storage
        void InitializeStorage();
        void FinalizeStorage();
        //insert a new element to storage
        void InsertElement(ElementEntity element);
        //returns elements storage status report
        ElementStorageStatus GetStatus();
    }
}
