using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projetASP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Home()
        {
            ViewBag.Current = "Home";
            ViewData["title"] = "Home";
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.Current = "Index";
            ViewData["title"] = "Index";
            return View(); 
        }
        public ActionResult Statistiques()
        {
            ViewBag.Current = "Statistiques";
            ViewData["title"] = "Statistiques";
            return View(); 
        }
    }
}