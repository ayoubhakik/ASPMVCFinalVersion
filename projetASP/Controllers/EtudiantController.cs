using projetASP.DAL;
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
        EtudiantContext s = new EtudiantContext();
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
        public ActionResult Modification([Bind(Include = "cne,nationalite,email,phone,gsm,address,ville,dateNaiss")] Etudiant etudiant,string Update,String choix1,String choix2,String choix3)
        {
            ViewBag.Current = "Modification";

            /*Update name of buttom if user click in Upload l image seule va etre modifie 
             

             */
            if (ModelState.IsValid)
            {
                Etudiant etudiants = etudiantContext.etudiants.Find(etudiant.cne);
                
                if (Request.Files.Count > 0 && Update=="Upload")
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
                    etudiants.choix= choix1 + choix2 + choix3;
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