using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projetASP.Controllers
{
    public class DepartementController : Controller
    {
        // GET: Departement
        public ActionResult Home()
        {
            ViewBag.Current = "home";
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.Current = "index";
            return View();
        }

        public ActionResult ImporterEtudiants()
        {
            ViewBag.Current = "importerEtudiants";
            return View();
        }
        public ActionResult ImporterNotes()
        {
            ViewBag.Current = "importerNotes";
            return View();
        }
        public ActionResult AttributionFiliere()
        {
            ViewBag.Current = "attributionFiliere";
            return View();
        }
        public ActionResult Statistiques()
        {
            ViewBag.Current = "statistiques";
            return View();
        }
        public ActionResult Visualiser()
        {
            ViewBag.Current = "visualiser";
            return View();
        }
    }
}