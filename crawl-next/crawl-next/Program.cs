using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace crawl_next
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawlerasync();
        }

        private static async Task startCrawlerasync()
        {
            try
            {
                var url = "https://stackoverflow.com/";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                WebResponse response = request.GetResponse();
                var responseString = new
                StreamReader(response.GetResponseStream()).ReadToEnd();

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(responseString);

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

        }
    }
}
