using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Filmweb.Models
{
    public class Rate
    {
        public Rate()
        {
        }

        public Rate(int iD, int rateScore)
        {
            ID = iD;
            RateScore = rateScore;
        }

        public int ID { get; set; }
        [Range(1, 5)]
        public int RateScore { get; set; }
       

    }
}