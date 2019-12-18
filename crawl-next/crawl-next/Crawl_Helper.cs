using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using crawl_next.Data_Context;
using System.Text.RegularExpressions;

namespace crawl_next
{
    class Crawl_Helper
    {
        internal const string tagsUrl = "https://stackoverflow.com/tags?page=1&tab=popular";
        internal const string questionsUrl = "https://stackoverflow.com/questions/tagged/{0}?tab={1}&page={2}&pagesize={3}";
        internal const string baseUrl = "https://stackoverflow.com";
        public static List<string> getTags()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(tagsUrl);
            var node = htmlDoc.GetElementbyId("tags-browser");
            List<string> tags = new List<string>();

            foreach (HtmlNode node1 in node.SelectNodes("//*[@class=\"tag-cell\"]/a"))
            {
                if (node1.HasAttributes)
                {
                    foreach (var nodeAtt in node1.Attributes.AttributesWithName("href"))
                    {
                        tags.Add(nodeAtt.Value.Replace("/questions/tagged/", ""));
                    }
                }
            }

            return tags;
        }

        public static List<string> getQuestions(string tags)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(tagsUrl);
            var node = htmlDoc.GetElementbyId("tags-browser");
            List<string> questionUrl = new List<string>();

            foreach (HtmlNode node1 in node.SelectNodes("//*[@class=\"question-summary\"]/div[contains(@class, 'summary')]/h3/a"))
            {
                if (node1.HasAttributes)
                {
                    foreach (var nodeAtt in node1.Attributes.AttributesWithName("href"))
                    {
                        questionUrl.Add(nodeAtt.Value);
                    }
                }
            }

            return questionUrl;
        }

        public static void getQuestionDetials(string url)
        {
            DataContext db = new DataContext();
            // Instantiate the regular expression object.
            Regex r = new Regex(@"^\/questions\/(\d*/)(\w*-*)*", RegexOptions.IgnoreCase);


            if (!db.documents.Any(x => x.url == url))
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(url);
                var node = htmlDoc.GetElementbyId("content");

                Console.WriteLine(" Going to get " + url);

                db.documents.Add(new Models.Document() { url = url, html = node.OuterHtml });
                db.SaveChanges();

                foreach (HtmlNode node1 in node.SelectNodes("//a[@href]"))
                {
                    if (node1.HasAttributes)
                    {
                        foreach (var nodeAtt in node1.Attributes.AttributesWithName("href"))
                        {
                            if (r.IsMatch(nodeAtt.Value))
                            {
                                getQuestionDetials(baseUrl + r.Matches(nodeAtt.Value)[0]);
                            }
                        }
                    }
                }
            }
        }

        public static string getDataFormUrl(string urlAddress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            return data;
        }
    }
}
