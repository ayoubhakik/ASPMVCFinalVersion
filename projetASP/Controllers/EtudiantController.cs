﻿using projetASP.DAL;
using projetASP.Models;
using Rotativa;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.UI;


namespace projetASP.Controllers
{
    public class EtudiantController : Controller
    {

        // GET: Etudiant
        EtudiantContext etudiantContext = new EtudiantContext();
        private string[] ImageEx = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".svg" };

        public ActionResult Index()
        {
            ViewBag.Current = "Home";

            if (UserValide.IsValid() && UserValide.IsStudent())
            {

                return View();
            }
            else
            {
                return RedirectToAction("Authentification1", "User");
            }
           
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



           
            if (UserValide.IsValid() && UserValide.IsStudent())
            {
                Etudiant etudiants = etudiantContext.etudiants.Find(Session["userId"]);

                return View(etudiants);
            }
            else
            {
                return RedirectToAction("Authentification1", "User");
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modification([Bind(Include = "cne,nationalite,email,phone,gsm,address,ville,dateNaiss,lieuNaiss")] Etudiant etudiant, string Update, String choix1, String choix2, String choix3)
        {
            ViewBag.Current = "Modification";

            /*Update name of buttom if user click in Upload l image seule va etre modifie 
             
             */
      
                Etudiant etudiants = etudiantContext.etudiants.Find(etudiant.cne);

            if (Request.Files.Count > 0 && Update == "Upload")
            {
                //Recupere le fichier est le sauvegarder dans /image/
                HttpPostedFileBase file = Request.Files[0];
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                ViewBag.exte = extension;
                if (fileName != "" && ImageEx.Contains(extension) == true)
                {
                    fileName = etudiants.nom + DateTime.Now.ToString("yymmssfff") + extension;
                    etudiants.photo_link = fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    file.SaveAs(fileName);
                    etudiants.Modified = true;
                    etudiantContext.SaveChanges();
                    return View(etudiants);


                }
                else
                {
                    ViewBag.err = " vous devez selectionner une image";
                    return View(etudiants);

                }

            }

            else
            {
                ViewBag.err = null;

                //si clicke sur les valider les modification 
                etudiants.Modified = true;
                etudiants.Choix = choix1 + choix2 + choix3;
                etudiants.nationalite = etudiant.nationalite;
                etudiants.email = etudiant.email;
                etudiants.phone = etudiant.phone;
                etudiants.address = etudiant.address;
                etudiants.gsm = etudiant.gsm;
                etudiants.address = etudiant.address;
                etudiants.ville = etudiant.ville;
                etudiants.dateNaiss = etudiant.dateNaiss;
                etudiants.lieuNaiss = etudiant.lieuNaiss;
                etudiantContext.SaveChanges();
                return RedirectToAction("SendEmailToUser");

            }
        }
        //****************************************************************************************************************************

       


        public ActionResult Consulter()
        {
            ViewBag.Current = "Consulter";
            if (UserValide.IsValid() && UserValide.IsStudent())
            {
                Etudiant etudiants = etudiantContext.etudiants.Find(Session["userId"]);

                return View(etudiants);
            }
            else
            {
                return RedirectToAction("Authentification1", "User");
            }
        }

        public ActionResult Deconnecter()
        {
            Session["userId"] = null;
            Session["cin"] = null;
            Session["nom"] = null;
            Session["prenom"] = null;
            Session["role"] = null;
            Session.Abandon();
            return RedirectToAction("Authentification1", "User");

        }

        //pour Imprimer le pdf

        public ActionResult PrintConsultation()
        {
            Etudiant etudiants = etudiantContext.etudiants.Find(Session["userId"]);
            var q = new ViewAsPdf("RecuEtudiant", etudiants);
            if (UserValide.IsValid() && UserValide.IsStudent())
            {
                return q;
            }
            else
            {
                return RedirectToAction("Authentification1", "User");
            }
        }



        [HttpGet]



        public ActionResult Inscription()
        {
            EtudiantContext db = new EtudiantContext();
            ViewBag.Delai = db.settings.FirstOrDefault().Delai;
            ViewBag.DatedeRappel = db.settings.FirstOrDefault().DatedeRappel;
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
        public ActionResult Inscription(Etudiant student, string choix1, string choix2, string choix3)
        {

            ViewBag.prenom = new SelectList(etudiantContext.etudiants, "cne", "prenom");
            ViewBag.nom = new SelectList(etudiantContext.etudiants, "cne", "nom");
            EtudiantContext db = new EtudiantContext();
            ViewBag.Delai = db.settings.FirstOrDefault().Delai;
            ViewBag.DatedeRappel = db.settings.FirstOrDefault().DatedeRappel;
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
                var e = etudiantContext.etudiants.Where(x => x.cne == student.cne && x.nom == student.nom && x.prenom == student.prenom).FirstOrDefault();

                if (e == null)
                {
                    ViewBag.message = "Les informations que vous avez entrez ne correspondent à aucun étudiant !";
                    return View();
                }
                else if (e.Validated == true)
                {
                    ViewBag.message = "Cet étudiant est déjà inscrit.";
                    return View();
                }
                else
                {
                    e.Validated = true;
                    e.password = student.password;
                    e.nationalite = student.nationalite;
                    e.email = student.email;
                    e.phone = student.phone;
                    e.gsm = student.gsm;
                    e.address = student.address;
                    e.ville = student.ville;
                    e.typeBac = student.typeBac;
                    e.anneeBac = student.anneeBac;
                    e.noteBac = student.noteBac;
                    e.mentionBac = student.mentionBac;
                    e.dateNaiss = student.dateNaiss;
                    e.lieuNaiss = student.lieuNaiss;
                    e.Choix = choix1 + choix2 + choix3;
                    etudiantContext.SaveChanges();

                    return RedirectToAction("SendEmailToUser1",new {email=e.email.ToString(),nom=e.nom.ToString(), prenom=e.prenom.ToString() });
                }
            }
            /*else
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

        }*/
            else return View();
        }
    

        public ActionResult SendEmailToUser()
        {
            bool Result = false;
            Etudiant etudiants = etudiantContext.etudiants.Find(Session["userId"]);
            string email = etudiants.email;
            string subject = "Modification";
            ViewBag.nom = etudiants.nom;
            ViewBag.prenom = etudiants.prenom;
            Result = SendEmail(email, subject, "<p> Hello"+" "+ @ViewBag.nom+" "+ @ViewBag.prenom +",<br/>some modifications had been done <br />Verify your account </p>" +
                "<button color='blue'><a href='localhost:localhost:52252/User/Authentification1'>Cliquer ici!</a></button>");
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
        public ActionResult SendEmailToUser1(String email,String nom ,String prenom)
        {
            bool Result = false;        
            string subject = "Inscription";
            Result = SendEmail(email, subject, "<p> Hello" + " " + nom + " " + prenom + ",<br/>Vous avez inscrit sur la plateforme d'ensas <br />Veuillez verifier votre compte </p>" +
                "<button color='blue'><a href='localhost:localhost:52252/User/Authentification1'>Cliquer ici!</a></button>");
            if (Result == true)
            {
                Json(Result, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Authentification1","User");
            }
            return View();
        }
        public bool SendEmail1(String toEmail, string subject, string EmailBody)
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
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
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