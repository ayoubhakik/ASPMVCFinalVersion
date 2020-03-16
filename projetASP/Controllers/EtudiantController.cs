using projetASP.DAL;
using projetASP.Models;
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


        EtudiantContext s = new EtudiantContext();
        

        [HttpGet]

        public ActionResult Inscription()
        {
            ViewBag.prenom = new SelectList(s.etudiants, "id", "prenom");
            ViewBag.nom = new SelectList(s.etudiants, "id", "nom");
            ViewBag.lieuNaiss = new SelectList(s.etudiants, "id", "lieuNaiss");
            ViewBag.nationalite = new SelectList(s.etudiants, "id", "nationalite");
            ViewBag.ville = new SelectList(s.etudiants, "id", "ville");
            ViewBag.typeBac = new SelectList(s.etudiants, "id", "typeBac");
            ViewBag.mentionBac = new SelectList(s.etudiants, "id", "mentionBac");
            


            return View();
        }

        [HttpPost]
        public ActionResult Inscription(Etudiant student)
        {
            if (ModelState.IsValid)
            {
                var e = (Etudiant)s.etudiants.Where(x => x.cin == student.cin);
                if (e == null)
                {
                    ViewBag.message = "Les informations que vous avez entrez ne correspondent à aucun étudiant !";
                    return View();
                }
                //update
                else 
                {
                    e.validated = true;
                    return null;
                }
                    
            }
            else return View();
        }

    }
}