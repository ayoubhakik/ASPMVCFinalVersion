﻿using projetASP.DAL;
using projetASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;

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
            Etudiant etudiants = etudiantContext.etudiants.Find("125");

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
                        return RedirectToAction("SendEmailToUser");
                        
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

            ViewBag.prenom = new SelectList(etudiantContext.etudiants, "cne", "prenom");
            ViewBag.nom = new SelectList(etudiantContext.etudiants, "cne", "nom");
           
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

            ViewBag.prenom = new SelectList(etudiantContext.etudiants, "cne", "prenom");
            ViewBag.nom = new SelectList(etudiantContext.etudiants, "cne", "nom");
            
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
                    ViewBag.message = "Les informations que vous avez entrez ne correspondent à aucun étudiant !";
                    return View();
                }
                //update
                else
                {
                    if (!student.password.Equals(e.password))
                    {
                        ViewBag.message = "Mot de pass incorrect !";
                        return View();
                    }

                    else
                    {           
                        e.validated = true;                          
                        etudiantContext.SaveChanges();
                        return null;
                    }
                }

            }
            else return View();
        }

        public ActionResult SendEmailToUser()
        {
            bool Result = false;
            Etudiant etudiants = etudiantContext.etudiants.Find("125");
            string email = etudiants.email;
            string subject = "Modification";
            ViewBag.nom = etudiants.nom;
            ViewBag.prenom = etudiants.prenom;
            Result = SendEmail(email, subject, "<p> Hi"+" "+ @ViewBag.nom+" "+ @ViewBag.prenom +",<br/>some modifications had been done <br />Verify your account </p>");
            if (Result == true)
            {
                Json(Result, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Modification");
            }
            return View();
        }
       public bool SendEmail(String toEmail,string subject,string EmailBody)
        {
            try
            {
                String senderEmail = WebConfigurationManager.AppSettings["senderEmail"];
                String senderPassword = WebConfigurationManager.AppSettings["senderPassword"];
               /* WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.UserName = sendereEmail;
                WebMail.Password = senderPassword;
                WebMail.Send(to: toEmail, subject: subject, body: EmailBody, isBodyHtml: true);*/
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail,senderPassword);
                MailMessage Message = new MailMessage(senderEmail, toEmail, subject, EmailBody);
                Message.IsBodyHtml = true;
                Message.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(Message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}