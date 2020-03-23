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
        // GET: Departement
        public ActionResult Home()
        {
            ViewBag.Current = "home";
            return View();
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
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
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
                            e.anneeBac = Convert.ToDateTime(DateTime.Now);
                            e.dateNaiss = Convert.ToDateTime(DateTime.Now);
                            /*
                            e.noteBac = Convert.ToDouble(workSheet.Cells[rowIterator, 13].Value);
                            e.mentionBac = workSheet.Cells[rowIterator, 14].Value.ToString();

                            e.lieuNaiss = workSheet.Cells[rowIterator, 16].Value.ToString();
                            */
                            db.etudiants.Add(e);
                            Console.WriteLine(" out ......");

                        }
                        db.SaveChanges();
                    }
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
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
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
                        db.SaveChanges();
                    }
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

                //the maximum number for each class will be the total/4
                int indexInfo = 0;
                int indexGtr = 0;
                int indexIndus = 0;
                int indexGpmc = 0;
                //the max numbers
                int maxInfo = list.Count / 4;
                int maxGtr= list.Count / 4;
                int maxIndus = list.Count / 4;
                int maxGpmc = list.Count/4 + (list.Count%4);


                for (int i=0;i<list.Count;i++)
                {
                    //verification de l'etudiant si deja a choisi une filiere sinon on va lui attribuer la derniere filiere (gpmc->indus->gtr->info)
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

                                    indexGtr++; ; break;
                                }
                            }
                            if (choice[j] == 'D')
                            {
                                if (indexIndus < maxIndus)
                                {
                                    list[i].idFil = 3;
                                    choosen = true;

                                    indexIndus++; ; break;
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
                        }
                        if (!choosen)
                        {
                            list[i].idFil = 4;
                            indexGpmc++;
                        }



                    }
                    
                        
                    
                }

                //list =list.OrderBy(e => (e.noteFstYear+e.noteSndYear)/2);
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
            System.Web.Helpers.Chart chart=new System.Web.Helpers.Chart(width:1000,height:600, theme: ChartTheme.Vanilla);
            chart.AddSeries(chartType: "Column", xValue: vx, yValues: vy);
            chart.Write("png");
            return null;
        }
    }

    
}