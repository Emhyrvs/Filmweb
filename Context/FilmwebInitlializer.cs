using Antlr.Runtime.Tree;
using Filmweb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace Filmweb.FilmwebContextt
{
    public class FilmwebInitlializer : DropCreateDatabaseIfModelChanges<FilmwebContext>
    {
        protected override void Seed(FilmwebContext context)
        {
            var rewiews = new List<Review>
            {
                new Review{ID=1,Title="Super",Content="Nigdy nic lepszego nie widziałem " },
                 new Review{ID=2,Title="żenada ",Content="Nigdy nic gorszego nie widziałem " },
                   new Review{ID=3,Title="Arcydzieło  ",Content="Arcydzieło kinomatofrafii!!! " }

            };


            rewiews.ForEach(m => context.Reviews.Add(m));
            context.SaveChanges();
            var movies = new List<Movie>
            {
                new Movie{ ID=1,Name="Dark Knigth",Director="Christofer Nolan",Length=120,Reviews=new List<Review> {rewiews[1]},Obrazek="DarkKinght.jpg"},
                new Movie{ID=2,Name="Two Towers-Lord of the Rings",Director="Peter Jackson",Length=300,Reviews=new List<Review> {rewiews[2]},Obrazek="LOTR1.jpg" }

            };
            movies.ForEach(m => context.Movies.Add(m));
            context.SaveChanges();
            var series = new List<Series>
            {
                new Series{ ID=1,Name="True Detective",Director="Nic Pizzolatto ", Length=60,NumberOfSeasons=3,NumberOfEpisode=8,Reviews=new List<Review> {rewiews[1]},Obrazek="true.jpg"},
                new Series{ID=2,Name="Sherlock",Director="Steven Moffat, Mark Gatiss",Length=120,NumberOfSeasons=3,NumberOfEpisode=3,Reviews=new List<Review> {rewiews[1]},Obrazek="Sherlock.jpg"  }

            };
            series.ForEach(m => context.TvSeries.Add(m));
            context.SaveChanges();
            var actors = new List<Actor>
            {
                new Actor{ID=1,Name="Christian",Surname="Bale",Age=40,Movies=new List<Movie>{movies[1] },Series=new List<Series>{series[0] },Obrazek="Bale.jpg"  }
                
            };
            actors.ForEach(m => context.Actors.Add(m));
            context.SaveChanges();
            var profiles = new List<Profile>
            {
                new Profile{ID=1,UserName="maciejzakrzewski1998@gmail.com"  },
                new Profile{ID=2,UserName="Admin@gmail.com"}

            };
            profiles.ForEach(m => context.Profiles.Add(m));
            context.SaveChanges();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole("Admin"));
            var user = new ApplicationUser { UserName = "Admin@gmail.com" };
            String password = "Admin1";
            userManager.Create(user, password);
            userManager.AddToRole(user.Id, "Admin");


        }
    }
}