using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Filmweb.FilmwebContextt;
using Filmweb.Models;
using PagedList;

namespace Filmweb.Controllers
{
    public class SeriesController : Controller
    {
        private FilmwebContext db = new FilmwebContext();

        // GET: TvSeries
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OcenaSortParm = sortOrder == "Ocena" ? "Ocena_desc" : "Ocena";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var series = from s in db.TvSeries
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                series = series.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    series = series.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    series = series.OrderBy(s => s.Rate);
                    break;
                case "date_desc":
                    series = series.OrderByDescending(s => s.Rate);
                    break;
                default:
                    series = series.OrderBy(s => s.ID);
                    break;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(series.ToPagedList(pageNumber, pageSize));
        }

        // GET: TvSeries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pomoc pomoc= new Pomoc();
            pomoc.actors = db.Actors.ToList();
            pomoc.series = db.TvSeries.Find(id);

            return View(pomoc);

        }
        public ActionResult Addrecenzje(string Id, string Title, string Content)
        {
            Series series = db.TvSeries.Find(Int32.Parse(Id));
            int rc = series.Reviews.FindLastIndex(a => a.ID > 0);
            rc++;
            series.Reviews.Add(new Review
            {
                ID = rc,
                Title = Title,
                Content = Content
            });
            db.Entry(series).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details" + "/" + Id);
            return View();
        }
        public ActionResult Addoceny(String Id, string rate)
        {
            Series series = db.TvSeries.Find(Int32.Parse(Id));
            int rc = series.Rates.FindLastIndex(a => a.ID > 0);
            rc++;
            series.Rates.Add(new Rate
            {
                ID = rc,
                RateScore = Int32.Parse(rate)

            });


            int liczba = 0;
            int r = 0;
            foreach (var rate1 in series.Rates)
            {
                r += rate1.RateScore;

                liczba++;
            }
            if (liczba != 0)
            {
                r /= liczba;
            }
            series.Rate = r;
            db.Entry(series).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details" + "/" + Id);
            return View();
        }
        public ActionResult Addaktora(String Idactor, String IdSeries)
        {
            Actor actor = db.Actors.Find(Int32.Parse(Idactor));
            Series series = db.TvSeries.Find(Int32.Parse(IdSeries));
            actor.Series.Add(series);
            db.Entry(actor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details" + "/" + IdSeries);

            return View();
        }

        // GET: TvSeries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TvSeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Director,Length,NumberOfSeasons,NumberOfEpisode,Obrazek")] Series series)
        {
            HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
            if (file != null && file.ContentLength > 0)
            {
                series.Obrazek = System.Guid.NewGuid().ToString();
                series.Obrazek = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + series.Obrazek);
                    
                    }
                    
              
                db.TvSeries.Add(series);
                db.SaveChanges();
                return RedirectToAction("Index");
            

            return View(series);
        }
       
        // GET: TvSeries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Series series = db.TvSeries.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        // POST: TvSeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Director,Length,NumberOfSeasons,NumberOfEpisode")] Series series)
        {
            if (ModelState.IsValid)

            {
                HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
                if (file != null && file.ContentLength > 0)
                {
                    series.Obrazek = System.Guid.NewGuid().ToString();
                    series.Obrazek = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + series.Obrazek);

                }

                db.Entry(series).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(series);
        }

        // GET: TvSeries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Series series = db.TvSeries.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        // POST: TvSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Series series = db.TvSeries.Find(id);
            db.TvSeries.Remove(series);
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
