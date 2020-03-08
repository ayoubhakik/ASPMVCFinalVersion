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
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImporterEtudiants()
        {
            return View();
        }
        public ActionResult ImporterNotes()
        {
            return View();
        }
        public ActionResult AttributionFiliere()
        {
            return View();
        }
        public ActionResult Statistiques()
        {
            return View();
        }
        public ActionResult Visualiser()
        {
            return View();
        }
    }
}