using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projetASP.Models;
namespace projetASP.Controllers
{
    public class DepartementController : Controller
    {
        // GET: Departement
        public ActionResult Home()
        {
            ViewBag.Current = "home";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Index()
        {

            ViewBag.Current = "index";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        public ActionResult ImporterEtudiants()
        {
            ViewBag.Current = "importerEtudiants";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult ImporterNotes()
        {
            ViewBag.Current = "importerNotes";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult AttributionFiliere()
        {
            ViewBag.Current = "attributionFiliere";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Statistiques()
        {
            ViewBag.Current = "statistiques";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Visualiser()
        {
            ViewBag.Current = "visualiser";
            if (UserValide.IsValid())
            {
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
    }
}