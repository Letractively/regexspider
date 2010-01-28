using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegExSpider.Spider
{
    public static class RegEx
    {
        public static string GetRegexMatch(string data, string regex)
        {
            Regex searchedRegex = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.ECMAScript | RegexOptions.Compiled);
            Match match = searchedRegex.Match(data);

            return match.Groups[1].Value;
        }

        private static MatchCollection GetAllRegexMatches(string data, string regex)
        {
            Regex searchedRegex = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.ECMAScript | RegexOptions.Compiled);                       
            return searchedRegex.Matches(data);
        }

        public static List<string> GetAllStrings(string data, string regex)
        {
            List<string> retList = new List<string>();
            MatchCollection mc = GetAllRegexMatches(data, regex);            

            foreach (Match m in mc)
            {
                if (string.IsNullOrEmpty(m.Value) == false)
                    retList.Add(m.Groups[1].Value);
            }

            return retList;
        }

        public static List<string> GetWebPageLinks(string rootUrl, string data)
        {
            List<string> retList = new List<string>();
            Uri rootUri = new Uri(rootUrl);
            MatchCollection mc = GetAllRegexMatches(data, "href=\"(?<LinkURL>.*?)\"");
            GroupCollection gc;

            foreach (Match m in mc)
            {
                gc = m.Groups;

                Uri result;
                if (Uri.TryCreate(rootUri, gc["LinkURL"].Value, out result))
                {
                    if (!retList.Contains(result.AbsoluteUri) && result.AbsoluteUri.StartsWith(rootUri.AbsoluteUri))
                    {
                        retList.Add(result.AbsoluteUri);
                    }
                }
            }

            return retList;
        }
    }
}
