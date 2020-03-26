//using OfficeOpenXml;
using OfficeOpenXml;
using projetASP.DAL;
using projetASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace projetASP.Controllers
{
    
    public class DepartementController : Controller
    {
        public ActionResult DeleteAllStudents()
        {
            if (UserValide.IsValid())
            {
                    EtudiantContext db = new EtudiantContext();
                    for (int i = 0; i < db.etudiants.ToList().Count; i++)
                    {
                            db.etudiants.Remove(db.etudiants.ToList()[i]);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        public ActionResult Search(string cne)
        {
            if (UserValide.IsValid())
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
            if (UserValide.IsValid())
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
            if (UserValide.IsValid())
            {
                if (id!=null)
                {
                    EtudiantContext db = new EtudiantContext();
                    db.etudiants.Find(id).Redoubler = true;
                    db.SaveChanges();
                    ViewBag.Current = "Corbeille";

                    return RedirectToAction("Corbeille");
                }
                else return RedirectToAction("Authentification", "User");

            }
            else
                return RedirectToAction("Authentification", "User");
        }
        public ActionResult Corbeille()
        {
            if (UserValide.IsValid())
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
            if (UserValide.IsValid())
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
            if (UserValide.IsValid())
            {
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
            
            if (UserValide.IsValid())
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
            if (UserValide.IsValid())
            {
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
            if (UserValide.IsValid())
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
            if (UserValide.IsValid())
            {
                EtudiantContext db = new EtudiantContext();
                List<Etudiant> list = db.etudiants.OrderByDescending(e => (e.noteFstYear + e.noteSndYear) / 2).ToList();
                //list =list.OrderBy(e => (e.noteFstYear+e.noteSndYear)/2);

                return View(list);
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        public ActionResult Attribution()
        {
            ViewBag.Current = "attributionFiliere";
            if (UserValide.IsValid())
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
                        char[] chiffr = (list[i].choix).ToCharArray();

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

                //the max numbers]
                int maxInfo = (list.Count-nbrRedoublant) / 4;
                int maxGtr = (list.Count - nbrRedoublant) / 4;
                int maxIndus = (list.Count - nbrRedoublant) / 4;
                int maxGpmc = (list.Count - nbrRedoublant) / 4;

                if (info>=indus && info >= gtr && info >= gpmc)
                {
                    maxInfo+= +((list.Count - nbrRedoublant) % 4);
                }
                if (indus >= info && indus >= gtr && indus >= gpmc)
                {
                     maxIndus += ((list.Count - nbrRedoublant) % 4);
                }
                if (gpmc >= indus && gpmc >= gtr && gpmc >= indus)
                {
                     maxGpmc  += ((list.Count - nbrRedoublant) % 4); 
                }
                else
                {
                     maxGtr += ((list.Count - nbrRedoublant) % 4); 
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

                    if (list[i].choix != null && !list[i].Redoubler )

                    if (list[i].choix != null)

                    {
                        //parse to a table of chars
                        char[] choice = list[i].choix.ToCharArray();
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

                db.SaveChanges();
                return RedirectToAction("AttributionFiliere");
            }
            else
                return RedirectToAction("Authentification", "User");
        }

        public ActionResult Statistiques()
        {
            ViewBag.Current = "statistiques";

            if (UserValide.IsValid())
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

                    if (list[i].Redoubler)
                    {
                        if (list[i].choix == null)
                        {

                            //un etudiant avec null dans choix alors on va l'es ajouter dans le reste
                            nbrReste++;
                        }
                        //sinon on va traiter les choix comme ca
                        else
                        {

                            char[] chiffr = (list[i].choix).ToCharArray();

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

                    if (list[i].choix==null)
                    {
                        //un etudiant avec null dans choix alors on va l'es ajouter dans le reste
                        nbrReste++;
                    }
                    //sinon on va traiter les choix comme ca
                    else
                    {
                        char[] chiffr = (list[i].choix).ToCharArray();

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
                ViewBag.indusP = Convert.ToDouble(indus) / Convert.ToDouble(nbrTotal) * 100;
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
                if (list[i].choix == null)
                {
                    //un etudiant avec null dans choix alors on va l'es ajouter dans le reste
                    nbrReste++;
                }
                //sinon on va traiter les choix comme ca
                else
                {
                    char[] chiffr = (list[i].choix).ToCharArray();

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