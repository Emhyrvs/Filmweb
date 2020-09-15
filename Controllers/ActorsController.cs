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
    public class ActorsController : Controller
    {
        private FilmwebContext db = new FilmwebContext();

        // GET: Actors
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.FilmySortParm = sortOrder == "Filmy" ? "Filmy_desc" : "Filmy";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var actors = from s in db.Actors
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                actors = actors.Where(s => s.Name.Contains(searchString) || s.Surname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    actors = actors.OrderByDescending(s => s.Name);
                    break;
                case "Filmy":
                    actors = actors.OrderBy(s => s.Movies.Count);
                    break;
                case "Filmy_desc":
                    actors = actors.OrderByDescending(s => s.Movies.Count);
                    break;
                default:
                    actors = actors.OrderBy(s => s.Surname);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(actors.ToPagedList(pageNumber, pageSize));
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFilm([Bind(Include = "ID,Name,Surname,Age")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }
        public ActionResult AddFilm()
        {

          
            return View(db.Movies.ToList());
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();

        }
        
        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Surname,Age,Obrazek")] Actor actor)
        {


            HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
            if (file != null && file.ContentLength > 0)
            {
                //actor.Obrazek = System.Guid.NewGuid().ToString();
                actor.Obrazek = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + actor.Obrazek);

            }


            db.Actors.Add(actor);
            db.SaveChanges();
            return RedirectToAction("Index");


           return View(actor);


           
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Surname,Age")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = db.Actors.Find(id);
            db.Actors.Remove(actor);
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
