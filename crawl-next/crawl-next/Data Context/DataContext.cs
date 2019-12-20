using crawl_next.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crawl_next.Data_Context
{
    class DataContext:DbContext
    {
        public DataContext():base("DefaultConnection")
        {

        }

        public DbSet<Document> documents { get; set; }

        public DbSet<MainDocument> mainDocuments { get; set; }
    }
}
