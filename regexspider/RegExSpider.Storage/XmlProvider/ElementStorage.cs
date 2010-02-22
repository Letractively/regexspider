using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RegExSpider.Storage.Entities;
using System.IO;

namespace RegExSpider.Storage.XmlProvider
{
    public class ElementStorage : IElementStorage
    {
        private XmlTextWriter m_XmlTextWriter;
        private int m_Stored = 0;

        private object m_SyncXmlWriter = new object();

        #region IElementStorage Members

        public void InitializeStorage(string[] args)
        {
            File.Delete("elements.xml");
            m_XmlTextWriter = new XmlTextWriter("elements.xml", Encoding.UTF8);

            m_XmlTextWriter.Formatting = Formatting.Indented;
            m_XmlTextWriter.Indentation = 1;
            m_XmlTextWriter.IndentChar = ' ';

            m_XmlTextWriter.WriteStartDocument();
            m_XmlTextWriter.WriteStartElement("Elements");
        }

        public void FinalizeStorage()
        {
            lock (m_SyncXmlWriter)
            {
                if (m_XmlTextWriter.WriteState != WriteState.Closed)
                {
                    m_XmlTextWriter.WriteEndElement();
                    m_XmlTextWriter.WriteEndDocument();
                    m_XmlTextWriter.Close();
                }
            }
        }

        public void InsertElement(ElementEntity element)
        {
            lock (m_SyncXmlWriter)
            {
                m_XmlTextWriter.WriteStartElement("Element");

                m_XmlTextWriter.WriteStartElement("Name");
                m_XmlTextWriter.WriteString(element.Name);
                m_XmlTextWriter.WriteEndElement();
                m_XmlTextWriter.WriteStartElement("Value");
                m_XmlTextWriter.WriteString(element.Value);
                m_XmlTextWriter.WriteEndElement();

                m_XmlTextWriter.WriteStartElement("Fields");

                foreach (var field in element.Fields)
                {
                    m_XmlTextWriter.WriteStartElement("Field");

                    m_XmlTextWriter.WriteStartElement("Name");
                    m_XmlTextWriter.WriteString(field.Value.Name);
                    m_XmlTextWriter.WriteEndElement();

                    m_XmlTextWriter.WriteStartElement("Value");
                    m_XmlTextWriter.WriteString(field.Value.Value);
                    m_XmlTextWriter.WriteEndElement();

                    m_XmlTextWriter.WriteEndElement();
                }

                m_XmlTextWriter.WriteEndElement();

                m_XmlTextWriter.WriteEndElement();
            }

            m_Stored++;
        }

        public ElementStorageStatus GetStatus()
        {
            ElementStorageStatus status = new ElementStorageStatus();
            status.Stored = m_Stored;
            return status;
        }

        #endregion
    }
}
