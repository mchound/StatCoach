using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StatCoach.Controllers
{
    public class ClubController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = RouteData.Values["club"] ?? "Ingen klubb hittades";
            ViewBag.Title = (ViewBag.Title as string).Replace('-', ' ');
            if (ViewBag.Title != "diffet")
                return RedirectToRoute("Default", RouteData.Values);

            return View();
        }

    }
}
