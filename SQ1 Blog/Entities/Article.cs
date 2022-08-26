using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Entities
{
    public class Article 
    {

        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        //[Required]
        //public string ArticleContent { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public bool IsImported { get; set; }

        public ApplicationUser User { get; set; }
    }
}
