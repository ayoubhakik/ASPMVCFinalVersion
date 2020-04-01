//using OfficeOpenXml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using projetASP.DAL;
using projetASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;

namespace projetASP.Controllers
{
    
    public class DepartementController : Controller
    {
        
        public void EnvoyerLesFilieres()
        {
            
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                for (int i = 0; i < db.etudiants.ToList().Count; i++)
                {
                    if (db.etudiants.ToList()[i].email!=null)
                    {
                        string body = "<div border='2px black solid'><h1 color='red'>Bonjour Mr/Mme " + db.etudiants.ToList()[i].nom + " " + db.etudiants.ToList()[i].prenom + "</h1>" +
                                                    "<p>Apres avoir faire l'attribution des filieres, on vient de vous informer que votre filiere est : " + db.Filieres.Find(db.etudiants.ToList()[i].idFil).nomFil + "</p><br/>" +
                                                    "<button color='blue'><a href='localhost:localhost:52252/User/Authentification1'>Cliquer ici!</a></button>" +
                                                    "</div>";
                        Boolean Result = SendEmail(db.etudiants.ToList()[i].email, "Information a propos la filiere attribuer ", body);
                        if (Result == true)
                        {
                            Json(Result, JsonRequestBehavior.AllowGet);
                        }
                    }
                        

                    
                }
            }
            

        }
        /*
         * on a pas les emails des etudiants non valide
        public ActionResult EnvoyerNotification()
        {
            if (UserValide.IsValid())
            {
                EtudiantContext db = new EtudiantContext();
                for (int i = 0; i < db.etudiants.ToList().Count; i++)
                {
                    //envoi d'un email pour lesles etudiants qu ont pas fait la validation des comptes
                    if (!db.etudiants.ToList()[i].Validated)
                    {
                        string body = "<h1>Bonjour mr/mme "+ db.etudiants.ToList()[i].nom+" "+db.etudiants.ToList()[i].prenom+"</h1>" +
                            "<p>essayer de valider votre compte pour choisir une filiere</p><br/>" +
                            "<a href='le lien ici'>Cliquer ici!</a>" +
                            "";
                        Boolean Result = SendEmail(db.etudiants.ToList()[i].email, "Notification pour la Validation ", body);
                        
                    }
                }
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        */
        public bool SendEmail(String toEmail, string subject, string EmailBody)
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

        public ActionResult DeleteAllStudents()
        {
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                    EtudiantContext db = new EtudiantContext();
                    for (int i = 0; i < db.etudiants.ToList().Count; i++)
                    {
                            db.etudiants.Remove(db.etudiants.ToList()[i]);
                    }
                    db.settings.First().Attributted = false;
                    db.settings.First().importEtudiant = false;
                    db.settings.First().importNote = false;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        public ActionResult Search(string cne)
        {
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                Etudiant e = db.etudiants.Find(cne);
                ViewBag.error = false;
                if (e==null)
                {
                    ViewBag.error =true;
                    return View();
                }
                return View(e);
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        //suppression des etudiants importes mais pas les redoublants
        public ActionResult DeleteImportedStudents()
        {
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                for (int i=0;i<db.etudiants.ToList().Count;i++)
                {
                    if (db.etudiants.ToList()[i].Redoubler == false)
                    {
                        db.etudiants.Remove(db.etudiants.ToList()[i]);
                    }
                }

                db.settings.First().Attributted = false;
                db.settings.First().importEtudiant = false;
                db.settings.First().importNote = false;

                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Authentification", "User");

        }

        //suppression des etudiants (placer les etudiants redoublants dans la corbeille)
        public ActionResult SupprimerEtudiant(string id)
        {
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                if (id!=null)
                {
                    EtudiantContext db = new EtudiantContext();
                    db.etudiants.Find(id).Redoubler = true;
                    db.SaveChanges();
                    ViewBag.Current = "Index";

                    return RedirectToAction("Index");
                }
                else return RedirectToAction("Authentification", "User");

            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Corbeille()
        {
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();


                ViewBag.Current = "Corbeille";

                return View(db.etudiants.ToList());
            }
            else
                return RedirectToAction("Authentification", "User");

        }

        public ActionResult Setting()
        {
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();

                
                db.SaveChanges();
                ViewBag.Current = "Setting";

                return View("Setting");
            }
            else
                return RedirectToAction("Authentification", "User");

        }

        // GET: Departement

        [HttpPost]
        public ActionResult Setting(DateTime dateNotification, DateTime dateAttribution)
        {
            if (UserValide.IsValid() && UserValide.IsAdmin()){ 
                EtudiantContext db = new EtudiantContext();

                if (dateNotification != null)
                {
                    db.settings.FirstOrDefault().DatedeRappel = dateNotification;
                }
                if (dateAttribution != null)
                {
                    db.settings.FirstOrDefault().Delai = dateNotification;
                }
                
                db.SaveChanges();
                ViewBag.Current = "Setting";

                return View("Setting");
            }
            else
                return RedirectToAction("Authentification", "User");



        }
        public ActionResult Index()
        {
            
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();

                
                ViewBag.Current = "index";
                List<Etudiant> list = db.etudiants.ToList();

                return View(list);
            }
            else
                return RedirectToAction("Authentification", "User");
 

            
        }

        public ActionResult ImporterEtudiants()
        {
            ViewBag.Current = "importerEtudiants";
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {

                EtudiantContext db = new EtudiantContext();
                if (!db.settings.FirstOrDefault().importEtudiant)
                {
                    return View();
                }
                ViewBag.err = true;
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        [HttpPost]
        public ActionResult ImporterEtudiantExcel(HttpPostedFileBase excelFile)
        {
            if (Request != null)
            {

                EtudiantContext db = new EtudiantContext();
                HttpPostedFileBase file = Request.Files["excelfile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName) && (file.FileName.EndsWith("xls")|| file.FileName.EndsWith("xlsx")) )
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        Console.WriteLine("before entering ......");
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            Console.WriteLine(" entering ......");
                            Etudiant e = new Etudiant();
                            e.nom = workSheet.Cells[rowIterator, 1].Value.ToString();

                            e.prenom = workSheet.Cells[rowIterator, 2].Value.ToString();

                            //e.nationalite = workSheet.Cells[rowIterator, 3].Value.ToString();

                            e.cin = workSheet.Cells[rowIterator, 3].Value.ToString();

                            e.cne = workSheet.Cells[rowIterator, 4].Value.ToString();
                            /*
                            e.email = workSheet.Cells[rowIterator, 6].Value.ToString();

                            e.phone = workSheet.Cells[rowIterator, 7].Value.ToString();


                            e.gsm = workSheet.Cells[rowIterator, 8].Value.ToString();

                            e.address = workSheet.Cells[rowIterator, 9].Value.ToString();

                            e.ville = workSheet.Cells[rowIterator,10].Value.ToString();


                            e.typeBac = workSheet.Cells[rowIterator, 11].Value.ToString();*/

                            //e.anneeBac = Convert.ToDateTime(DateTime.Now);

                            //e.anneeBac = Convert.ToDateTime(DateTime.Now);

                            e.dateNaiss = Convert.ToDateTime(DateTime.Now);
                            /*
                            e.noteBac = Convert.ToDouble(workSheet.Cells[rowIterator, 13].Value);
                            e.mentionBac = workSheet.Cells[rowIterator, 14].Value.ToString();

                            e.lieuNaiss = workSheet.Cells[rowIterator, 16].Value.ToString();
                            */
                            db.etudiants.Add(e);
                            Console.WriteLine(" out ......");

                        }
                        db.settings.First().importEtudiant = true;
                        db.SaveChanges();
                        for (int i = 0; i < db.etudiants.ToList().Count; i++)
                        {
                            db.etudiants.ToList()[i].Choix = "FDT";
                        }
                        db.SaveChanges();

                    }
                }
                else
                {
                    ViewBag.errI = true;
                    return View("ImporterEtudiants");
                }
            }
            return RedirectToAction("Index");
            //return View("Index");
        }


        public ActionResult ImporterNotes()
        {
            ViewBag.Current = "importerNotes";
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                if (db.settings.FirstOrDefault().importEtudiant)
                {
                    return View();
                }
                ViewBag.err = true;
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        [HttpPost]
        public ActionResult ImporterNoteExcel(HttpPostedFileBase excelFile)
        {
            if (Request != null)
            {
                EtudiantContext db = new EtudiantContext();
                HttpPostedFileBase file = Request.Files["excelfile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName) && (file.FileName.EndsWith("xls") || file.FileName.EndsWith("xlsx")))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        Console.WriteLine("before entering ......");
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            Console.WriteLine(" entering ......");
                            Etudiant e = db.etudiants.Find(workSheet.Cells[rowIterator, 1].Value.ToString());
                            e.noteFstYear = Convert.ToDouble(workSheet.Cells[rowIterator, 2].Value);

                            e.noteSndYear = Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value);

                           

                            //db.etudiants.Add(e);
                            Console.WriteLine(" out ......");

                        }
                        db.settings.First().importNote = true;
           
                       db.SaveChanges();
                    }
                }
                else
                {
                    ViewBag.errI = true;
                    return View("ImporterNotes");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult AttributionFiliere()
        {
            ViewBag.Current = "attributionFiliere";
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                List<Etudiant> list = db.etudiants.OrderByDescending(e => (e.noteFstYear + e.noteSndYear) / 2).ToList();

                int total =0;
                for (int i=0;i<db.etudiants.ToList().Count;i++)
                {
                    if (!db.etudiants.ToList()[i].Redoubler)
                    {
                        total++;
                    }
                }
                ViewBag.total = total;
                int info = 0, indus = 0, gpmc = 0, gtr = 0;
                for (int i = 0; i < db.etudiants.ToList().Count; i++)
                {
                    if (!db.etudiants.ToList()[i].Redoubler && db.etudiants.ToList()[i].Validated)
                    {
                        char[] chiffr = (list[i].Choix).ToCharArray();

                        if (chiffr[0] == 'F')
                        {
                            info++;
                        }
                        if (chiffr[0] == 'P')
                        {
                            gpmc++;
                        }
                        if (chiffr[0] == 'T')
                        {
                            gtr++;
                        }
                        if (chiffr[0] == 'D')
                        {
                            indus++;
                        }
                    }
                    

                }

                //il faut envoyer les nbr par defaut pour les textboxes
                //il faut classer les filieres par nbr de demande
                ////////partie statistique

                //variable pour les nombre totale et le reste qui n'a pas choisi les filieres
                int nbrTotal =total;

                
                //initialisation des Maxs
                int maxInfo = total / 4;
                int maxGtr = total / 4;
                int maxIndus = total / 4;
                int maxGpmc = total / 4;

                int diff =total%4;
                Boolean symbol1 = false;
                Boolean symbol2 = false;
                Boolean symbol3 = false;
                Boolean symbol4 = false;
                Boolean taken = false;
                for (int i = 0; i < diff; i++)
                {
                    if (info >= indus && info >= gtr && info >= gpmc && !symbol1 && !taken)
                    {
                        symbol1 = true;
                        taken = true;
                        maxInfo += 1;
                    }
                    if (indus >= info && indus >= gtr && indus >= gpmc && !symbol2 &&!taken)
                    {
                        symbol2 = true;
                        taken = true;

                        maxIndus += 1;
                    }
                    if (gpmc >= indus && gpmc >= gtr && gpmc >= indus && !symbol3 && !taken)
                    {
                        symbol3 = true;
                        taken = true;

                        maxGpmc += 1; 
                    }
                    if (gtr >= indus && gtr >= gpmc && gtr >= info && !symbol4 && !taken)
                    {
                        taken = true;

                        symbol4 = true;
                        maxGtr += 1; break;
                    }
                   taken = false;
                }

                ViewBag.info = maxInfo;
                ViewBag.gtr = maxGtr;
                ViewBag.gpmc = maxGpmc;
                ViewBag.indus = maxIndus;
                //list =list.OrderBy(e => (e.noteFstYear+e.noteSndYear)/2);
                if (db.settings.FirstOrDefault().importNote)
                {
                    return View(list);
                }
                ViewBag.err = true;
                return View(list);
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        [HttpPost]
        public ActionResult AttributionFiliere(string infoMax,string indusMax,string gtrMax,string gpmcMax)
        {
           
            ViewBag.Current = "attributionFiliere";
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                //return a  list sorted in a desendent way
                List<Etudiant> list = db.etudiants.OrderByDescending(e => (e.noteFstYear + e.noteSndYear) / 2).ToList();
                int total = 0;
                for (int i = 0; i < db.etudiants.ToList().Count; i++)
                {
                    if (!db.etudiants.ToList()[i].Redoubler)
                    {
                        total++;
                    }
                }
                int maxInfo = total / 4;
                int maxGtr = total / 4;
                int maxIndus = total / 4;
                int maxGpmc = total / 4;

                int diff = total % 4;
                Boolean symbol1 = false;
                Boolean symbol2 = false;
                Boolean symbol3 = false;
                Boolean symbol4 = false;
                Boolean taken = false;
                int info = 0, indus = 0, gpmc = 0, gtr = 0;
                for (int i = 0; i < db.etudiants.ToList().Count; i++)
                {
                    if (!db.etudiants.ToList()[i].Redoubler && db.etudiants.ToList()[i].Validated)
                    {
                        char[] chiffr = (list[i].Choix).ToCharArray();

                        if (chiffr[0] == 'F')
                        {
                            info++;
                        }
                        if (chiffr[0] == 'P')
                        {
                            gpmc++;
                        }
                        if (chiffr[0] == 'T')
                        {
                            gtr++;
                        }
                        if (chiffr[0] == 'D')
                        {
                            indus++;
                        }
                    }


                }

                //il faut envoyer les nbr par defaut pour les textboxes
                //il faut classer les filieres par nbr de demande
                ////////partie statistique

                //variable pour les nombre totale et le reste qui n'a pas choisi les filieres
                int nbrTotal = total;


                //initialisation des Maxs

                for (int i = 0; i < diff; i++)
                {
                    if (info >= indus && info >= gtr && info >= gpmc && !symbol1 && !taken)
                    {
                        symbol1 = true;
                        taken = true;
                        maxInfo += 1;
                    }
                    if (indus >= info && indus >= gtr && indus >= gpmc && !symbol2 && !taken)
                    {
                        symbol2 = true;
                        taken = true;

                        maxIndus += 1;
                    }
                    if (gpmc >= indus && gpmc >= gtr && gpmc >= indus && !symbol3 && !taken)
                    {
                        symbol3 = true;
                        taken = true;

                        maxGpmc += 1;
                    }
                    if (gtr >= indus && gtr >= gpmc && gtr >= info && !symbol4 && !taken)
                    {
                        taken = true;

                        symbol4 = true;
                        maxGtr += 1; break;
                    }
                    taken = false;
                }

                //changer les maxs si la departement a saisi des valeurs
                if (infoMax!=null && indusMax != null && gpmcMax != null && gtrMax != null)
                {
                    try
                    {

                        maxInfo = Convert.ToInt32(infoMax);
                        maxIndus = Convert.ToInt32(indusMax);
                        maxGtr = Convert.ToInt32(gtrMax);
                        maxGpmc = Convert.ToInt32(gpmcMax);
                        if (maxInfo+maxIndus+maxGtr+maxGpmc!=total)
                        {
                            ViewBag.error2 = true;
                            return View();
                        }
                        
                    }
                    catch (Exception e)
                    {

                    }

                }
                
                






                int indexInfo = 0;
                int indexGtr = 0;
                int indexIndus = 0;
                int indexGpmc = 0;
                for (int i=0;i<list.Count;i++)
                {
                    //verification de l'etudiant si deja a choisi une filiere sinon on va lui attribuer la derniere filiere (gpmc->indus->gtr->info)

                    if (!list[i].Redoubler )
                    {
                        if (list[i].Validated)
                        {
                            //parse to a table of chars
                            char[] choice = list[i].Choix.ToCharArray();
                            //verify the frst case which is if we have F=info
                            Boolean choosen = false;

                            for (int j = 0; j < 3; j++)
                            {

                                if (choice[j] == 'F')
                                {
                                    if (indexInfo < maxInfo)
                                    {
                                        list[i].idFil = 1;
                                        choosen = true;
                                        indexInfo++; break;
                                    }
                                }
                                if (choice[j] == 'T')
                                {
                                    if (indexGtr < maxGtr)
                                    {
                                        list[i].idFil = 2;
                                        choosen = true;

                                        indexGtr++; break;
                                    }
                                }
                                if (choice[j] == 'D')
                                {
                                    if (indexIndus < maxIndus)
                                    {
                                        list[i].idFil = 3;
                                        choosen = true;

                                        indexIndus++; break;
                                    }

                                }
                                if (choice[j] == 'P')
                                {
                                    if (indexGpmc < maxGpmc)
                                    {
                                        list[i].idFil = 4;
                                        choosen = true;
                                        indexGpmc++; break;
                                    }
                                }
                                if (choosen)
                                {
                                    j = 3;
                                }
                                if (!choosen && j == 2)
                                {
                                    list[i].idFil = 4;
                                    choosen = true;
                                    indexGpmc++; break;
                                }
                            }
                        }
                        else
                        {
                            taken = false;
                            if (info >= indus && info >= gtr && info >= gpmc &&  !taken && (indexInfo < maxInfo))
                            {
                                taken = true;
                                list[i].idFil = 1;
                                indexInfo += 1;
                            }
                            if (indus >= info  && indus >= gtr && indus >= gpmc && !taken && (indexIndus < maxIndus))
                            {
                                taken = true;
                                list[i].idFil = 3;
                                indexIndus += 1;
                            }
                            if (gtr >= indus && gtr >= info && gtr >= gpmc && !taken && (indexGtr < maxGtr))
                            {
                                taken = true;
                                list[i].idFil = 2;
                                indexGtr += 1;
                            }
                            if (gpmc >= indus && gpmc >= gtr && gpmc >= info && !taken && (indexGpmc < maxGpmc))
                            {
                                taken = true;
                                list[i].idFil = 4;
                                indexGpmc += 1;
                            }
                        }
                        
                        



                    }
                    
                        
                    
                }

                //list =list.OrderBy(e => (e.noteFstYear+e.noteSndYear)/2);

                db.settings.First().Attributted = true;
                //envoi d'un msg qui contient la filiere attribuer pour tous chaque etudiants 
                db.SaveChanges();
                EnvoyerLesFilieres();

                return RedirectToAction("AttributionFiliere");
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        public ActionResult Statistiques()
        {
            ViewBag.Current = "statistiques";

            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                //essayons de retourner tous les etudiants
                EtudiantContext db = new EtudiantContext();
                List<Etudiant> list = db.etudiants.ToList();
                //initialisation des compteurs des filieres
                int info=0, indus = 0, gtr = 0, gpmc = 0;

                //variable pour les nombre totale et le reste qui n'a pas choisi les filieres
                int nbrTotal=list.Count, nbrReste=0;

                for (int i=0;i<nbrTotal;i++)
                {

                    if (!list[i].Redoubler)
                    {
                        if (!list[i].Validated)
                        {

                            //un etudiant avec null dans choix alors on va l'es ajouter dans le reste
                            nbrReste++;
                        }
                        //sinon on va traiter les choix comme ca
                        else
                        {

                            char[] chiffr = (list[i].Choix).ToCharArray();

                            if (chiffr[0] == 'F')
                            {
                                info++;
                            }
                            if (chiffr[0] == 'P')
                            {
                                gpmc++;
                            }
                            if (chiffr[0] == 'T')
                            {
                                gtr++;
                            }
                            if (chiffr[0] == 'D')
                            {
                                indus++;
                            }
                        }

                    }
                    else
                    {
                        nbrTotal--;
                    }

                    
                   

                }
                ViewBag.nbrTotal = nbrTotal;
                ViewBag.nbrReste = nbrReste;
                ViewBag.info = info;
                ViewBag.gtr = gtr;
                ViewBag.gpmc = gpmc;
                ViewBag.indus = indus;
                //les pourcentages
                ViewBag.nbrTotalP =  Convert.ToDouble(nbrTotal)/ Convert.ToDouble(nbrTotal) * 100;
                ViewBag.nbrResteP = Convert.ToDouble(nbrReste) / Convert.ToDouble(nbrTotal) * 100;
                ViewBag.infoP = Convert.ToDouble(info)/ Convert.ToDouble(nbrTotal) * 100;
                ViewBag.gtrP = Convert.ToDouble(gtr)/ Convert.ToDouble(nbrTotal) * 100;
                ViewBag.gpmcP = Convert.ToDouble(gpmc) / Convert.ToDouble(nbrTotal) * 100;
                ViewBag.indusP =Convert.ToDouble(indus) / Convert.ToDouble(nbrTotal) * 100;
                return View();
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Visualiser()
        {
            ViewBag.Current = "visualiser";
            if (UserValide.IsValid() && UserValide.IsAdmin())
            {
                EtudiantContext db = new EtudiantContext();
                
                return View(db.etudiants.ToList());
            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Chart()
        {
            //essayons de retourner tous les etudiants
            EtudiantContext db = new EtudiantContext();
            List<Etudiant> list = db.etudiants.ToList();
            //initialisation des compteurs des filieres
            int info = 0, indus = 0, gtr = 0, gpmc = 0;

            //variable pour les nombre totale et le reste qui n'a pas choisi les filieres
            int nbrTotal = list.Count, nbrReste = 0;

            for (int i = 0; i < nbrTotal; i++)
            {
                if (list[i].Choix == null)
                {
                    //un etudiant avec null dans choix alors on va l'es ajouter dans le reste
                    nbrReste++;
                }
                //sinon on va traiter les choix comme ca
                else
                {
                    if (list[i].Validated)
                    {
                        char[] chiffr = (list[i].Choix).ToCharArray();

                        if (chiffr[0] == 'F')
                        {
                            info++;
                        }
                        if (chiffr[0] == 'P')
                        {
                            gpmc++;
                        }
                        if (chiffr[0] == 'T')
                        {
                            gtr++;
                        }
                        if (chiffr[0] == 'D')
                        {
                            indus++;
                        }
                    }
                    
                }

            }
            
            //les pourcentages
            //double nbrTotalP = Convert.ToDouble(nbrTotal) / Convert.ToDouble(nbrTotal) * 100;
            //double nbrResteP = Convert.ToDouble(nbrReste) / Convert.ToDouble(nbrTotal) * 100;
            double infoP = Convert.ToDouble(info) / Convert.ToDouble(nbrTotal) * 100;
            double gtrP = Convert.ToDouble(gtr) / Convert.ToDouble(nbrTotal) * 100;
            double gpmcP = Convert.ToDouble(gpmc) / Convert.ToDouble(nbrTotal) * 100;
            double indusP = Convert.ToDouble(indus) / Convert.ToDouble(nbrTotal) * 100;


            string[] vx = { "info", "indus", "gtr", "gpmc" };
            double[] vy ={infoP, indusP, gtrP, gpmcP };

            System.Web.Helpers.Chart chart=new System.Web.Helpers.Chart(width:900,height:400, theme: ChartTheme.Blue);

          
            chart.AddSeries(chartType: "Column", xValue: vx, yValues: vy);
            chart.Write("png");
            return null;
        }
        [HttpPost]
        public void ExtraireNonValide()
        {
            EtudiantContext students = new EtudiantContext();

            //Création de la page excel
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Sheet1");

            //Style des noms de colonnes
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Noms des colonnes
            worksheet.Cells[1, 1].Value = "Nom";
            worksheet.Cells[1, 2].Value = "Prenom";
            worksheet.Cells[1, 3].Value = "CIN";
            worksheet.Cells[1, 4].Value = "CNE";
            

            //Remplissage des cellules
            int rowIndex = 2;
            foreach (var student in students.etudiants.ToList())
            {
                worksheet.Cells[rowIndex, 1].Value = student.nom;
                worksheet.Cells[rowIndex, 2].Value = student.prenom;
                worksheet.Cells[rowIndex, 3].Value = student.cin;
                worksheet.Cells[rowIndex, 4].Value = student.cne;
                
                rowIndex++;


            }

            //Envoi du fichier dans par http
            using (var memoryStream = new MemoryStream())
            {
                Response.Clear();
                Response.ClearContent();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=EtudiantNonValideCompte.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.Clear();
                Response.End();
            }
        }
        [HttpGet]
        public void ExportExcel()
        {
            //Données à exporter
            EtudiantContext students = new EtudiantContext();

            //Création de la page excel
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Sheet1");

            //Style des noms de colonnes
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Noms des colonnes
            worksheet.Cells[1, 1].Value = "Nom";
            worksheet.Cells[1, 2].Value = "Prenom";
            worksheet.Cells[1, 3].Value = "CIN";
            worksheet.Cells[1, 4].Value = "CNE";
            worksheet.Cells[1, 5].Value = "Email"; 
            worksheet.Cells[1, 6].Value = "Date de naissance";
            worksheet.Cells[1, 7].Value = "Lieu de naissance";
            worksheet.Cells[1, 8].Value = "Nationalite";
            worksheet.Cells[1, 9].Value = "GSM";
            worksheet.Cells[1, 10].Value = "Tel fixe";
            worksheet.Cells[1, 11].Value = "Adresse";
            worksheet.Cells[1, 12].Value = "Ville";
            worksheet.Cells[1, 13].Value = "Type de bac";
            worksheet.Cells[1, 14].Value = "Annee de bac";
            worksheet.Cells[1, 15].Value = "Note de bac";
            worksheet.Cells[1, 16].Value = "Note de premiere annee";
            worksheet.Cells[1, 17].Value = "Note de deuxieme annee";
            worksheet.Cells[1, 18].Value = "Choix";
            worksheet.Cells[1, 19].Value = "Filiere affectee";
            worksheet.Cells[1, 20].Value = "Redoublant";

            //Remplissage des cellules
            int rowIndex = 2;
            foreach (var student in students.etudiants.ToList())
            {
                worksheet.Cells[rowIndex, 1].Value = student.nom;
                worksheet.Cells[rowIndex, 2].Value = student.prenom;
                worksheet.Cells[rowIndex, 3].Value = student.cin;
                worksheet.Cells[rowIndex, 4].Value = student.cne;
                worksheet.Cells[rowIndex, 5].Value = student.email;
                worksheet.Cells[rowIndex, 6].Value = student.dateNaiss;
                worksheet.Cells[rowIndex, 7].Value = student.lieuNaiss;
                worksheet.Cells[rowIndex, 8].Value = student.nationalite;
                worksheet.Cells[rowIndex, 9].Value = student.gsm;
                worksheet.Cells[rowIndex, 10].Value = student.phone;
                worksheet.Cells[rowIndex, 11].Value = student.address;
                worksheet.Cells[rowIndex, 12].Value = student.ville;
                worksheet.Cells[rowIndex, 13].Value = student.typeBac;
                worksheet.Cells[rowIndex, 14].Value = student.anneeBac;
                worksheet.Cells[rowIndex, 15].Value = student.noteBac;
                worksheet.Cells[rowIndex, 16].Value = student.noteFstYear;
                worksheet.Cells[rowIndex, 17].Value = student.noteSndYear;
                worksheet.Cells[rowIndex, 18].Value = student.choix;
                worksheet.Cells[rowIndex, 19].Value = student.idFil;
                worksheet.Cells[rowIndex, 20].Value = student.redoubler;
                rowIndex++;
               

            }

            //Envoi du fichier dans par http
            using (var memoryStream = new MemoryStream())
            {
                Response.Clear();
                Response.ClearContent();
                Response.ContentType ="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=testing.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.Clear();
                Response.End();
            }
            


            }
    }

    
}