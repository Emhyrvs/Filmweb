using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmweb.Models
{
    public class Series
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Director { get; set; }

        public int Length { get; set; }

        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisode { get; set; }
        public virtual List<Rate> Rates { get; set; }
        public virtual List<Review> Reviews { get; set; }

        public String Obrazek { get; set; }
        public int Rate { get; set; }
   



    }
}