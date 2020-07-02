using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Filmweb.Models
{
    public class Review
    {
        public Review()
        {
        }

        public Review(int iD, string title, string content)
        {
            ID = iD;
            Title = title;
            Content = content;
        }

        public int ID { get; set; }
        [MaxLength(20)]
        public String Title { get; set; }
        [MaxLength(200),MinLength(10)]
        public String Content { get; set; }
      
    }
}