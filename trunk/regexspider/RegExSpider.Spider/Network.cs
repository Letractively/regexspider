using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace RegExSpider.Spider
{
    public static class Network
    {
        public static string GetHTML(string url)
        {
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);                
                HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();
                StreamReader sr = new StreamReader(httpRes.GetResponseStream());

                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
