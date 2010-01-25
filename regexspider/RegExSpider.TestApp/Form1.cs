using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegExSpider.Configuration;
using RegExSpider.Storage.Entities;
using System.Diagnostics;

namespace RegExSpider.TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Spider.Spider spider = new RegExSpider.Spider.Spider();
                spider.OnReportStatus += new RegExSpider.Spider.Spider.ReportStatus(spider_OnReportStatus);
                spider.Initialize(ConfigReader.ReadSiteEntityXml(ofd.FileName), 10);
                spider.StartCrawling();                
            }
            
        }

        void spider_OnReportStatus(ElementStorageStatus elements, LinkStorageStatus links)
        {
            Debug.WriteLine("Stored elements: " + elements.Stored + " Queue links: " + links.Waiting + " Scanned links: " + links.Scanned);
        }
        
    }
}
