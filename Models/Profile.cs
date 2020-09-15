using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmweb.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public String UserName { get; set; }

        
            public virtual List<Movie> Movies { get; set; }
            public virtual List<Series> Series { get; set; }
        
        public virtual List<Review> Reviews { get; set; }
        public virtual List<Rate> Rates { get; set; }
        
    }
}