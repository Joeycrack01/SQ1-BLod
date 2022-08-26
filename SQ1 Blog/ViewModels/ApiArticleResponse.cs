using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.ViewModels
{
    public class ApiArticleResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("publication_date")]
        public DateTime PublicaionDate { get; set; }
    }

    public class ServiceResponse
    {
        public List<ApiArticleResponse> Data { get; set; }
    }
}
