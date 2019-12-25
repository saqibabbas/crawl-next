using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using crawl_next.Data_Context;
using HtmlAgilityPack;

namespace crawl_next
{
    class Program
    {
        static void Main(string[] args)
        {
            //startCrawlerasync();

            List<string> questionUrl = new List<string>();
            List<string> tags = Crawl_Helper.getTags();

            foreach (var item in tags)
            {
                questionUrl = Crawl_Helper.getQuestions(string.Format(Crawl_Helper.questionsUrl, item, "votes", 1, 50));

                foreach (var itemquestion in questionUrl)
                {
                    Console.WriteLine(itemquestion);
                    Crawl_Helper.getQuestionDetials(Crawl_Helper.baseUrl + itemquestion);
                }

            }

            //questionUrl = Crawl_Helper.getQuestions("https://stackoverflow.com/questions/tagged/c%23?tab=votes&pagesize=50");
            //https://stackoverflow.com/questions/114543/how-to-horizontally-center-a-div
            //
            //Crawl_Helper.getQuestionDetials("https://stackoverflow.com/questions/38187126/how-to-access-nested-div-based-on-class-name");



            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument htmlDoc = web.Load("https://stackoverflow.com/questions/12859891/error-unable-to-access-the-iis-metabase?page=1&tab=votes#tab-top");
            //var node = htmlDoc.GetElementbyId("content");
            //foreach (HtmlNode node1 in node.SelectNodes("//*[@class=\"pager-answers\"]/span[contains(@class, 'page-number')]"))
            //{
            //    Console.WriteLine(node1.InnerHtml);
            //}

        }
    }

}
