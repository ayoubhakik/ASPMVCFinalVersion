﻿using projetASP.DAL;
using projetASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace projetASP.Controllers
{
    public class EtudiantController : Controller
    {

        // GET: Etudiant
        EtudiantContext etudiantContext = new EtudiantContext();
        public ActionResult Index()
        {

            ViewBag.Current = "Home";

            return View();
        }





        //--------------------------------------------------------------------------------------------------------------------------
        //Modification 
        public ActionResult Modification()
        {
            ViewBag.Current = "Modification";
            ViewBag.check = "Checked";
            /* if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }*/
            Etudiant etudiants = etudiantContext.etudiants.Find("9qdq");

            if (etudiants == null)
            {
                return HttpNotFound();
            }

            return View(etudiants);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modification([Bind(Include = "cne,nationalite,email,phone,gsm,address,ville,dateNaiss")] Etudiant etudiant, string Update, String choix1, String choix2, String choix3)
        {
            ViewBag.Current = "Modification";

            /*Update name of buttom if user click in Upload l image seule va etre modifie 
             
             */
            if (ModelState.IsValid)
            {
                Etudiant etudiants = etudiantContext.etudiants.Find(etudiant.cne);

                if (Request.Files.Count > 0 && Update == "Upload")
                {
                    //Recupere le fichier est le sauvegarder dans /image/
                    HttpPostedFileBase file = Request.Files[0];
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    fileName = DateTime.Now.ToString("yymmssfff") + extension;
                    etudiants.photo_link = fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    file.SaveAs(fileName);

                    etudiantContext.SaveChanges();
                    return View(etudiants);
                }
                else
                {
                    //si clicke sur les valider les modification 
                    etudiants.choix = choix1 + choix2 + choix3;
                    etudiants.nationalite = etudiant.nationalite;
                    etudiants.email = etudiant.email;
                    etudiants.phone = etudiant.phone;
                    etudiants.address = etudiant.address;
                    etudiants.gsm = etudiant.gsm;
                    etudiants.address = etudiant.address;
                    etudiants.ville = etudiant.ville;
                    etudiants.dateNaiss = etudiant.dateNaiss;

                    etudiantContext.SaveChanges();
                    return View(etudiants);
                    // return RedirectToAction("Index");
                }

            }

            return View(etudiant);
        }

        //****************************************************************************************************************************


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

        [HttpGet]

        public ActionResult Inscription()
        {

            
            ViewBag.typeBac = new List<SelectListItem>
            {
                new SelectListItem {Text="Sciences Physiques et Chimiques", Value="1" },
                new SelectListItem {Text="Sciences Maths A", Value="2" },
                new SelectListItem {Text="Sciences Maths B", Value="3" },
                new SelectListItem {Text="Sciences et Technologies Electriques", Value="4" },
                new SelectListItem {Text="Sciences et Technologies Mécaniques", Value="5" }
            };
            ViewBag.mentionBac = new List<SelectListItem>
            {
                new SelectListItem {Text="Passable", Value="1" },
                new SelectListItem {Text="Assez bien", Value="2" },
                new SelectListItem {Text="Bien", Value="3" },
                new SelectListItem {Text="Très bien", Value="4" },
            };


            return View();
        }

        [HttpPost]
        public ActionResult Inscription(Etudiant student)
        {
           
            ViewBag.typeBac = new List<SelectListItem>
            {
                new SelectListItem {Text="Sciences Physiques et Chimiques", Value="1" },
                new SelectListItem {Text="Sciences Maths A", Value="2" },
                new SelectListItem {Text="Sciences Maths B", Value="3" },
                new SelectListItem {Text="Sciences et Technologies Electriques", Value="4" },
                new SelectListItem {Text="Sciences et Technologies Mécaniques", Value="5" }
            };
            ViewBag.mentionBac = new List<SelectListItem>
            {
                new SelectListItem {Text="Passable", Value="1" },
                new SelectListItem {Text="Assez bien", Value="2" },
                new SelectListItem {Text="Bien", Value="3" },
                new SelectListItem {Text="Très bien", Value="4" },
            };

            if (ModelState.IsValid)
            {
                var e = etudiantContext.etudiants.Where(x => x.cne == student.cne).FirstOrDefault();

                if (e == null)
                {
                    ViewBag.message = "Les informations que vous avez entrez ne correspondent a aucun etudiant !";
                    return View();
                }
                //update
                else
                {
                    if (student.cin.Equals(e.cin, StringComparison.InvariantCultureIgnoreCase) && student.nom.Equals(e.nom, StringComparison.OrdinalIgnoreCase) && student.prenom.Equals(e.prenom, StringComparison.OrdinalIgnoreCase))
                    {
                        e.validated = true;
                        e.password = student.password;
                        e.dateNaiss = student.dateNaiss;
                        e.lieuNaiss = student.lieuNaiss;
                        e.phone = student.phone;
                        e.gsm = student.gsm;
                        e.email = student.email;
                        e.address = student.address;
                        e.nationalite = student.nationalite;
                        e.ville = student.ville;
                        e.typeBac = student.typeBac;
                        e.anneeBac = student.anneeBac;
                        e.noteBac = student.noteBac;
                        e.mentionBac = student.mentionBac;
                        e.choix = student.choix;
                        etudiantContext.SaveChanges();
                        return RedirectToAction("Authentification","User");
                    }

                    else
                    {

                        ViewBag.message = "CIN, CNE, Nom ou Prenom incorrect !";
                        return View();
                    }
                }

            }
            else return View();
        }
    }
}