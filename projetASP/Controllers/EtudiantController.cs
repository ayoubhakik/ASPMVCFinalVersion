using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projetASP.Controllers
{
    public class EtudiantController : Controller
    {
        // GET: Etudiant
        public ActionResult Index()
        {
            ViewBag.Current = "Home";
            return View();
        }

        public ActionResult Modification()
        {
            ViewBag.Current = "Modification";
            return View();
        }

        public ActionResult Consulter()
        {
            ViewBag.Current = "Consulter";
            return View();
        }

        public ActionResult Deconnecter()
        {
            ViewBag.Current = "Deconnecter";
            return View("Index");
        }
    }
}