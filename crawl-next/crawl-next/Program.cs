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
                questionUrl=  Crawl_Helper.getQuestions(string.Format(Crawl_Helper.questionsUrl, item, "votes", 1, 50));

                foreach (var itemquestion in questionUrl)
                {
                    Console.WriteLine(itemquestion);
                }

            }

            //questionUrl = Crawl_Helper.getQuestions("https://stackoverflow.com/questions/tagged/c%23?tab=votes&pagesize=50");


            //Crawl_Helper.getQuestionDetials("https://stackoverflow.com/questions/7074/what-is-the-difference-between-string-and-string-in-c");
            //DataContext db = new DataContext();
            //foreach (var item in db.documents.Select(x => x.url))
            //{
            //    Console.WriteLine(item);
            //} 
        }
    }
}
