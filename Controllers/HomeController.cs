using System.Linq;
using System.Web.Mvc;
using Filmweb.FilmwebContextt;
using Filmweb.Models;
using Filmweb.FilmwebContextt;


namespace Filmweb.Controllers
{
    public class HomeController : Controller
    {
          private FilmwebContext db = new FilmwebContext();
        private object movie;
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
            IQueryable<Models.EnrollmentDateGroup> data = from movie in db.Movies
                                                   group movie by movie.Director into dateGroup
                                                   select new EnrollmentDateGroup()
                                                   {
                                                       Director = dateGroup.Key,
                                                       MovieCount = dateGroup.Count()
                                                   };
            return View(data.ToList());
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}