using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Filmweb.FilmwebContextt;
using Filmweb.Models;
using PagedList;

namespace Filmweb.Controllers
{
    public class MoviesController : Controller
    {
        private FilmwebContext db = new FilmwebContext();

        public static string Subject { get; private set; }
        public static string Body { get; private set; }

        // GET: Movies

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var movies = from s in db.Movies
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.Rate);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.Rate);
                    break;
                default:
                    movies = movies.OrderBy(s => s.Length);
                    break;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(movies.ToPagedList(pageNumber, pageSize));
            
        }
        public ActionResult Addrecenzje(string Id, string Title, string Content)
        {
           Movie movie = db.Movies.Find(Int32.Parse(Id));
            int rc = movie.Reviews.FindLastIndex(a=>a.ID>0);
            rc++;
            movie.Reviews.Add(new Review
            {  ID = rc,
            Title = Title , 
               Content = Content
            });
            db.Entry(movie).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details"+"/"+Id);
            return View();
        }
        public ActionResult Addoceny(String Id, string rate)
        {
            Movie movie = db.Movies.Find(Int32.Parse(Id));
            int rc = movie.Rates.FindLastIndex(a => a.ID > 0);
            rc++;
            movie.Rates.Add(new Rate
            {
                ID = rc,
                RateScore = Int32.Parse(rate)

            }) ;

            
            int liczba = 0;
            int r = 0;
            foreach (var rate1 in movie.Rates)
            {
                r += rate1.RateScore;

                liczba++;
            }
            if (liczba != 0)
            {
                r /= liczba;
            }
            movie.Rate = r;
            db.Entry(movie).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details" + "/" + Id);
            return View();
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pomoc pomocnicza = new Pomoc();
            pomocnicza.actors = db.Actors.ToList();
            pomocnicza.movie = db.Movies.Find(id);
            
            return View(pomocnicza);


        }
        public ActionResult Addaktora(String  Idactor,String  IdMovie )
        {
            Actor actor =db.Actors.Find(Int32.Parse(Idactor));
            Movie movie = db.Movies.Find(Int32.Parse(IdMovie));
            actor.Movies.Add(movie);
            db.Entry(actor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details" + "/" + IdMovie);
            
            return View();
        }

        // GET: Movies/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,Name,Director,Length,Obrazek")] Movie movie)
        {
              HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
            if (file != null && file.ContentLength > 0)
            {
                //actor.Obrazek = System.Guid.NewGuid().ToString();
                movie.Obrazek = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + movie.Obrazek);

            }


            db.Movies.Add(movie);
            db.SaveChanges();
            return RedirectToAction("Index");


            return View(movie);

           
        }
        

        // GET: Movies/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
       


        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Name,Director,Length")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
        
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
       
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
