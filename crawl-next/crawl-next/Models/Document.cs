using System.ComponentModel.DataAnnotations.Schema;

namespace crawl_next.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string url { get; set; }

        public string html { get; set; }

        public int page { get; set; }

        public int? mainDocumentId { get; set; }
        
        [ForeignKey("mainDocumentId")]
        public MainDocument mainDocument { get; set; }

    }
}
