using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SQ1Blog.ViewModels
{
    public class ArticleViewModel
    {

        public int ID { get; set; }

        public string Title { get; set; }

        public string ArticleContent { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CreatedBy { get; set; }

        public string Description { get; set; }
    }

    public class ArticleListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublicationDate { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }

    }
}
