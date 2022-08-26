using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Models
{
    public class AddArticlePageModel
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        //[Required]
        //public string ArticleContent { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        public string CreatedBy { get; set; }
    }
}
