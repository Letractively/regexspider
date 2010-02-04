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
    public partial class frmMain : Form
    {
        private int m_CrawlersNumber = 1;
        private delegate void UpdateStateDelegate(int waiting, int scanned, int elements);
        private UpdateStateDelegate UpdateStateInvoke;

        private Spider.Spider m_Spider;

        public frmMain()
        {
            InitializeComponent();
            UpdateStateInvoke = UpdateState;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCrawlersNum.Text, out m_CrawlersNumber) == false)
            {
                MessageBox.Show("Only round numbers as Crawlers number...", "Error");
                return;
            }

            m_Spider = new RegExSpider.Spider.Spider();
            m_Spider.OnReportStatus += new RegExSpider.Spider.Spider.ReportStatus(spider_OnReportStatus);
            m_Spider.Initialize(ConfigReader.ReadSiteEntityXml(txtConfig.Text), m_CrawlersNumber);
            m_Spider.StartCrawling();
        }

        void spider_OnReportStatus(ElementStorageStatus elements, LinkStorageStatus links)
        {
            this.Invoke(UpdateStateInvoke, links.Waiting, links.Scanned, elements.Stored);
        }

        private void UpdateState(int waiting, int scanned, int elements)
        {
            txtElementsFound.Text = elements.ToString();
            txtWaiting.Text = waiting.ToString();
            txtScanned.Text = scanned.ToString();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtConfig.Text = ofd.FileName;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_Spider.StopCrawling();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            m_Spider.PauseCrawling();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            m_Spider.ResumeCrawling();
        }


    }
}
