//using OfficeOpenXml;
using OfficeOpenXml;
using projetASP.DAL;
using projetASP.Models;
using System;
using System.Collections.Generic;
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
                int total=0;
                for (int i=0;i<db.etudiants.ToList().Count;i++)
                {
                    if (!db.etudiants.ToList()[i].Redoubler)
                    {
                        total++;
                    }
                }
                ViewBag.total = total;
                List<Etudiant> list = db.etudiants.OrderByDescending(e => (e.noteFstYear + e.noteSndYear) / 2).ToList();
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

                ////////partie statistique
                int info = 0, indus = 0, gtr = 0, gpmc = 0;

                //variable pour les nombre totale et le reste qui n'a pas choisi les filieres
                int nbrTotal = list.Count, nbrRedoublant = 0;

                for (int i = 0; i < nbrTotal; i++)
                {
                    if (list[i].Redoubler)
                    {
                        //un etudiant avec null dans choix alors on va l'es ajouter dans le reste
                        nbrRedoublant++;
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

                ViewBag.info = info;
                ViewBag.gtr = gtr;
                ViewBag.gpmc = gpmc;
                ViewBag.indus = indus;

                //the maximum number for each class will be the total/4
                int indexInfo = 0;
                int indexGtr = 0;
                int indexIndus = 0;
                int indexGpmc = 0;

                //initialisation des Maxs
                int maxInfo = (list.Count - nbrRedoublant) / 4;
                int maxGtr = (list.Count - nbrRedoublant) / 4;
                int maxIndus = (list.Count - nbrRedoublant) / 4;
                int maxGpmc = (list.Count - nbrRedoublant) / 4;

                //changer les maxs si la departement a saisi des valeurs
                if (infoMax!=null && indusMax != null && gpmcMax != null && gtrMax != null)
                {
                    try
                    {

                        maxInfo = Convert.ToInt32(infoMax);
                        maxIndus = Convert.ToInt32(indusMax);
                        maxGtr = Convert.ToInt32(gtrMax);
                        maxGpmc = Convert.ToInt32(gpmcMax);
                        
                    }
                    catch (Exception e)
                    {

                    }

                }
                 //si ladministration n'a pas entre le nbr alors on va diviser les nombres automatiquement
                
                    int diff = (list.Count - nbrRedoublant) - (maxGtr + maxIndus + maxInfo + maxGpmc);
                    Boolean symbol1 = false;
                    Boolean symbol2 = false;
                    Boolean symbol3 = false;
                    Boolean symbol4 = false;
                    for (int i = 0; i < diff; i++)
                    {
                        if (info >= indus && info >= gtr && info >= gpmc && !symbol1)
                        {
                            symbol1 = true;
                            symbol2 = false;
                            symbol3 = true;
                            symbol4 = true;
                            maxInfo += 1;
                        }
                        if (indus >= info && indus >= gtr && indus >= gpmc && !symbol2)
                        {
                            symbol1 = true;
                            symbol2 = true;
                            symbol3 = false;
                            symbol4 = true;
                            maxIndus += 1;
                        }
                        if (gpmc >= indus && gpmc >= gtr && gpmc >= indus && !symbol3)
                        {
                            symbol1 = true;
                            symbol2 = true;
                            symbol3 = true;
                            symbol4 = false;
                            maxGpmc += 1;
                        }
                        if (gtr >= indus && gtr >= gpmc && gtr >= info && !symbol4)
                        {
                            symbol1 = false;
                            symbol2 = true;
                            symbol3 = true;
                            symbol4 = true;
                            maxGtr += 1;
                        }
                    }


                    /*the max numbers
                    int maxInfo = list.Count / 4;
                    int maxGtr= list.Count / 4;
                    int maxIndus = list.Count / 4;
                    int maxGpmc = list.Count/4 + (list.Count%4);
                    */

                





                for (int i=0;i<list.Count;i++)
                {
                    //verification de l'etudiant si deja a choisi une filiere sinon on va lui attribuer la derniere filiere (gpmc->indus->gtr->info)

                    if (list[i].Choix != null && !list[i].Redoubler )
                    {
                        //parse to a table of chars
                        char[] choice = list[i].Choix.ToCharArray();
                        //verify the frst case which is if we have F=info
                        Boolean choosen = false;

                        for (int j=0;j<3;j++)
                        {

                            if (choice[j]=='F')
                            {
                                if (indexInfo < maxInfo)
                                {
                                    list[i].idFil = 1;
                                    choosen = true;
                                    indexInfo++;break;
                                }
                            }
                            if (choice[j] == 'T')
                            {
                                if (indexGtr < maxGtr)
                                {
                                    list[i].idFil = 2;
                                    choosen = true;

                                    indexGtr++;  break;
                                }
                            }
                            if (choice[j] == 'D')
                            {
                                if (indexIndus < maxIndus)
                                {
                                    list[i].idFil = 3;
                                    choosen = true;

                                    indexIndus++;  break;
                                }

                            }
                            if (choice[j] == 'P')
                            {
                                if (indexGpmc < maxGpmc)
                                {
                                    list[i].idFil = 4;
                                    choosen = true;
                                    indexGpmc++;break;
                                }
                            }
                            if (choosen)
                            {
                                j = 3;
                            }
                            if (!choosen && j==2 )
                            {
                                list[i].idFil = 4;
                                choosen = true;
                                indexGpmc++; break;
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
    }

    
}