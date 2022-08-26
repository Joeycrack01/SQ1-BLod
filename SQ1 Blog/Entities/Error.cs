using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Entities
{
    public class Error
    {
        public int ID { get; set; }
        public int TenantId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
