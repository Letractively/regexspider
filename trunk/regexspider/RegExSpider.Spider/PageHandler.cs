using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegExSpider.Storage.Entities;
using RegExSpider.Configuration;
using System.Threading;

namespace RegExSpider.Spider
{
    public class PageHandler
    {
        public delegate void HandlingFinished(LinkEntity link);
        public event HandlingFinished OnHandlingFinished = delegate { };

        public delegate void FoundLinks(List<string> links, int handlerDepth);
        public event FoundLinks OnFoundLinks = delegate { };

        public delegate void FoundElements(List<ElementEntity> elements);
        public event FoundElements OnFoundElements = delegate { };

        private SiteEntity m_SiteEntity;
        private string m_HtmlContent = string.Empty;

        private int m_ElementCounter = 0;

        private List<ElementEntity> m_Results;

        public PageHandler(SiteEntity ownerSite)
        {
            m_Results = new List<ElementEntity>();

            m_SiteEntity = ownerSite;
        }

        public void HandlePage(object state)
        {
            LinkEntity link = (LinkEntity)state;
            
            m_HtmlContent = Network.GetHTML(link.Url);

            List<string> foundUrls = RegEx.GetWebPageLinks(m_SiteEntity.RootUrl, m_HtmlContent);

            foreach (string noFollowRule in m_SiteEntity.NoFollowExpressions) //remove no follow urls.
            {
                foundUrls.RemoveAll(o => !string.IsNullOrEmpty(RegEx.GetRegexMatch(o, noFollowRule)));
            }

            OnFoundLinks(foundUrls,link.Depth);

            foreach (var extractionElement in m_SiteEntity.ExtractionElements) //extract the root elements
            {
                List<string> matches = GetElementsFromString(m_HtmlContent, extractionElement);

                foreach (var item in matches)
                {
                    Interlocked.Increment(ref m_ElementCounter);

                    ElementEntity newElement = new ElementEntity();
                    newElement.Id = m_ElementCounter;
                    newElement.Name = extractionElement.Name;
                    newElement.Value=item;

                    int fieldsCounter = 0;

                    foreach (var field in extractionElement.Fields)
                    {
                        string fieldMatch = GetElementFromString(newElement.Value, field.Value);

                        if (string.IsNullOrEmpty(fieldMatch) == false)
                        {
                            fieldsCounter++;

                            ElementEntity newField = new ElementEntity();
                            newField.Id = fieldsCounter;
                            newField.Name = field.Key;
                            newField.Value = fieldMatch;

                            newElement.Fields.Add(newField.Name, newField);
                        }
                    }

                    m_Results.Add(newElement);
                }
            }

            if (m_Results.Count > 0)
                OnFoundElements(m_Results);

            OnHandlingFinished(link);
        }        

        private List<string> GetElementsFromString(string data, ExtractionElement element)
        {
            return RegEx.GetAllStrings(data, element.RegEx);
        }

        private string GetElementFromString(string data, ExtractionElement element)
        {
            return RegEx.GetRegexMatch(data, element.RegEx);
        }
    }
}
