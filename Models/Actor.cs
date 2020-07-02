using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Filmweb.Models
{
    public class Actor
    {
        public int ID { get; set; }
        public String Name { get; set; }
        [MaxLength(30),MinLength(2)]
        public String  Surname { get; set; }
        [Range(1,150)]
        public int Age { get; set; }
        public virtual List<Movie>  Movies { get; set; }
        public virtual List<Series> Series { get; set; }
        public String Obrazek { get; set; }

    }
}