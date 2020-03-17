using OfficeOpenXml;
using projetASP.DAL;
using projetASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            EtudiantContext db = new EtudiantContext();

            Etudiant e = db.etudiants.Find("9qdq");
            e.Validated=true;
            db.SaveChanges();

            ViewBag.Current = "index";
            List<Etudiant> list = db.etudiants.ToList();
                return View(list);

            
        }

        public ActionResult ImporterEtudiants()
        {
            ViewBag.Current = "importerEtudiants";
            return View();
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

                            e.nationalite = workSheet.Cells[rowIterator, 3].Value.ToString();

                            e.cin = workSheet.Cells[rowIterator, 4].Value.ToString();

                            e.cne = workSheet.Cells[rowIterator, 5].Value.ToString();

                            e.email = workSheet.Cells[rowIterator, 6].Value.ToString();

                            e.phone = workSheet.Cells[rowIterator, 7].Value.ToString();


                            e.gsm = workSheet.Cells[rowIterator, 8].Value.ToString();

                            e.address = workSheet.Cells[rowIterator, 9].Value.ToString();

                            e.ville = workSheet.Cells[rowIterator,10].Value.ToString();


                            e.typeBac = workSheet.Cells[rowIterator, 11].Value.ToString();
                            e.anneeBac = Convert.ToInt32(workSheet.Cells[rowIterator, 12].Value); 
                            e.noteBac = Convert.ToDouble(workSheet.Cells[rowIterator, 13].Value);
                            e.mentionBac = workSheet.Cells[rowIterator, 14].Value.ToString();

                            e.dateNaiss =Convert.ToDateTime(workSheet.Cells[rowIterator, 15].Value) ;
                            e.lieuNaiss = workSheet.Cells[rowIterator, 16].Value.ToString();

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
            return View();
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
            return View();
        }
        public ActionResult Statistiques()
        {
            ViewBag.Current = "statistiques";
            return View();
        }
        public ActionResult Visualiser()
        {
            ViewBag.Current = "visualiser";
            return View();
        }
    }
}