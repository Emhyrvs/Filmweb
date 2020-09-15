using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Filmweb.FilmwebContextt;
using Filmweb.Models;

namespace Filmweb.Controllers
{
    public class RatesController : Controller
    {
        private FilmwebContext db = new FilmwebContext();

        // GET: Rates
        public ActionResult Index()
        {
            return View(db.Rates.ToList());
        }

        // GET: Rates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // GET: Rates/Create
        public ActionResult Create()
        {
            Rate model = new Rate();
            return View(model);
        }

        // POST: Rates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(String  id,[Bind(Include = "ID,RateScore")] Rate rate)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id.Contains("movie"))
            {
                String ids = id.TrimStart("movie".ToCharArray());
                int idm = 0;
                idm = Int32.Parse(ids);
                Movie movie = db.Movies.Find(idm);
              


                    movie.Rates.Add(rate);
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
                    db.Rates.Add(rate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
            }
            else
            {
                String ids = id.TrimStart("series".ToCharArray());
                int idm = 0;
                idm = Int32.Parse(ids);
                Series series = db.TvSeries.Find(idm);
              


                    series.Rates.Add(rate);
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
                    db.Rates.Add(rate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    
                
            }
            return View();
        }

        // GET: Rates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RateScore")] Rate rate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rate);
        }

        // GET: Rates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rate rate = db.Rates.Find(id);
            db.Rates.Remove(rate);
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
