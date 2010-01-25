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
            MatchCollection mc = GetAllRegexMatches(data, "href=\"(?<LinkURL>.*?)\"");
            GroupCollection gc;

            foreach (Match m in mc)
            {
                gc = m.Groups;
                if (!retList.Contains(rootUrl + gc["LinkURL"].Value))
                    retList.Add(rootUrl + gc["LinkURL"].Value);
            }

            return retList;
        }
    }
}
