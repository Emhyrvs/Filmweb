using Filmweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Filmweb.FilmwebContextt
{
    public class FilmwebContext:DbContext
    {
        public FilmwebContext():base("Defoult Connection")
        {

        }
        public DbSet<Movie>  Movies { get; set; }
       
        public DbSet<Series> TvSeries { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rate> Rates { get; set; }

        public System.Data.Entity.DbSet<Filmweb.Models.Profile> Profiles { get; set; }
    }
}