using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RegExSpider.Configuration
{
    public static class ConfigReader
    {
        public static SiteEntity ReadSiteEntityXml(string xmlPath)
        {
            SiteEntity siteEntity = new SiteEntity();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);

            XmlElement xmlElement = doc.DocumentElement;
            siteEntity.Name = xmlElement["Name"].InnerText;
            siteEntity.RootUrl = xmlElement["RootUrl"].InnerText;

            xmlElement = xmlElement["CrawlUrlsConfig"];
            siteEntity.MaxDepth = int.Parse(xmlElement["MaxDepth"].InnerText);

            foreach (XmlElement startPoint in xmlElement.ChildNodes)
            {
                if (startPoint.Name.Equals("StartPoint"))
                {
                    siteEntity.StartPointUrls.Add(startPoint.InnerText);
                }
            }

            xmlElement = xmlElement["NoFollow"];

            foreach (XmlElement regex in xmlElement.ChildNodes)
            {
                if (regex.Name.Equals("RegEx"))
                {
                    siteEntity.NoFollowExpressions.Add(regex.InnerText);
                }
            }

            xmlElement = doc.DocumentElement["PageExtractionElements"];

            foreach (XmlElement element in xmlElement.ChildNodes)
            {
                if (element.Name.Equals("Element"))
                {
                    ExtractionElement extractionElement = new ExtractionElement();
                    extractionElement.Name = element.Attributes["Name"].Value;
                    extractionElement.RegEx = element.Attributes["RegEx"].Value;

                    foreach (XmlElement field in element.ChildNodes)
                    {
                        if (field.Name.Equals("Field"))
                        {
                            ExtractionElement fieldElement = new ExtractionElement();
                            fieldElement.Name = field.Attributes["Name"].Value;
                            fieldElement.RegEx = field.Attributes["RegEx"].Value;

                            extractionElement.Fields.Add(fieldElement.Name, fieldElement);
                        }
                    }

                    siteEntity.ExtractionElements.Add(extractionElement);
                }
            }

            return siteEntity;
        }
    }
}
