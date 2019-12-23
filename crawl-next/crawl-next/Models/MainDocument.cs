using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crawl_next.Models
{
    public class MainDocument
    {
        public int Id { get; set; }

        public string url { get; set; }
        
        public List<Document> documents { get; set; }
    }
}
