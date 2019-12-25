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
        internal const string questionPostUrl = "https://stackoverflow.com{0}?page={1}&tab=votes#tab-top";

        static DataContext db = new DataContext();
        // Instantiate the regular expression object.
        static Regex r = new Regex(@"^\/questions\/(\d*/)(\w*-*)*", RegexOptions.IgnoreCase);

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
            HtmlDocument htmlDoc = web.Load(tags);
            var node = htmlDoc.GetElementbyId("mainbar");
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


            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return questionUrl;
        }

        public static void getQuestionDetials(string url, int callstack = 0)
        {
            if (!db.mainDocuments.Any(x => x.url == url) && callstack < 90)
            {
                HtmlNode node = null;
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument htmlDoc = web.Load(url);
                    node = htmlDoc.GetElementbyId("content");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("At Url :" + url + " At callStack : " + callstack);
                }
                if (node != null)
                {
                    List<int> pages = getPagination(node);

                    Models.MainDocument mainDocument = db.mainDocuments.Add(new Models.MainDocument { url = url });
                    db.documents.Add(new Models.Document() { url = url, html = node.OuterHtml, mainDocument = mainDocument, page = 1 });
                    db.SaveChanges();

                    Console.WriteLine(" Going to get " + url);

                    getPaginationData(pages, url, mainDocument.Id);

                    foreach (HtmlNode node1 in node.SelectNodes("//a[@href]"))
                    {
                        if (node1.HasAttributes)
                        {
                            foreach (var nodeAtt in node1.Attributes.AttributesWithName("href"))
                            {
                                if (r.IsMatch(nodeAtt.Value))
                                {
                                    getQuestionDetials(baseUrl + r.Matches(nodeAtt.Value)[0], callstack++);
                                }
                            }
                        }
                    }
                }
            }


            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Get Paging data of document is paging is found
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="url"></param>
        public static void getPaginationData(List<int> pages, string url, int mainDocumentId)
        {
            foreach (var item in pages)
            {
                HtmlWeb web = new HtmlWeb();
                string queryPart = string.Format("?page={0}&amp;tab=votes#tab-top", item.ToString());
                HtmlDocument htmlDoc = web.Load(url + queryPart);
                HtmlNode node = htmlDoc.GetElementbyId("content");

                Console.WriteLine("Started geting pagination request");

                db.documents.Add(new Models.Document() { url = url, html = node.OuterHtml, page = item, mainDocumentId = mainDocumentId });
                db.SaveChanges();
            }


            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
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

        public static List<int> getPagination(HtmlNode node)
        {
            List<int> paging = new List<int>();
            int pag = 0;

            HtmlNodeCollection elements = node.SelectNodes("*//span[contains(@class, 'page-number')]");
            if (elements != null)
            {
                foreach (HtmlNode node1 in elements)
                {
                    if (int.TryParse(node1.InnerText, out pag))
                    {
                        paging.Add(pag);
                    }
                }
                paging = paging.Where(x => x > 1).Distinct().ToList();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return paging;
        }
    }
}
