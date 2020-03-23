using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using projetASP.DAL;
using projetASP.Models;

namespace projetASP.Controllers
{

    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Authentification()
        {
            if (UserValide.IsValid())
            {

                return RedirectToAction("Index", "Departement");
            }
            else
                return View();

        }

        [HttpPost]
        public ActionResult Authentification(Departement login, string ReturnUrl = "")
        {
            String button = Request["loginBtn"];
            string message = "";
            if (button == "Login")
            {
                string userName = Request["userName"];
                string mdp = Request["mdp"];
                EtudiantContext dbset = new EtudiantContext();
                var userLogin = (from data in dbset.departements
                                 where data.username == userName && data.password == mdp
                                 select data).FirstOrDefault();
                if (userLogin != null)
                {
                    Session["userName"] = userLogin.username;
                    Session["NomDep"] = userLogin.nom_departement;
                    Session["EmailDep"] = userLogin.email;
                    Session["userId"] = userLogin.id_departement;
                    return RedirectToAction("Index", "Departement");
                }
                else if (userLogin == null)
                {

                    message = "Invalid username or password";
                    ViewBag.Message = message;
                    return View();
                }

            }

            return View();

        }
        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["NomDep"] = null;
            Session["EmailDep"] = null;
            Session["userId"] = null;

            return RedirectToAction("Authentification", "User");
        }

    


    
    [HttpGet]
    public ActionResult Authentification1()
    {
        if (UserValide.IsValid())
        {

            return RedirectToAction("Index", "Etudiant");
        }
        else
            return View();

    }

        [HttpPost]
        public ActionResult Authentification1(Etudiant login, string ReturnUrl = "")
        {
            String button = Request["loginBtn"];
            string message = "";
            if (button == "Login")
            {
                string cne = Request["cne"];
                string cin = Request["cin"];
                string mdp = Request["mdp"];
                EtudiantContext dbset = new EtudiantContext();
                var userLogin = (from data in dbset.etudiants
                                 where data.cne == cne && data.password == mdp && data.cin == cin && data.Validated == true
                                 select data).FirstOrDefault();
                if (userLogin != null)
                {
                    Session["cin"] = userLogin.cin;
                    Session["userId"] = userLogin.cne;
                    Session["nom"] = userLogin.nom;
                    Session["prenom"] = userLogin.prenom;
                    /* Session["nationalite"] = userLogin.nationalite;
                     Session["email"] = userLogin.email;
                     Session["phone"] = userLogin.phone;
                     Session["gsm"] = userLogin.gsm;
                     Session["address"] = userLogin.address;
                     Session["ville"] = userLogin.ville;
                     Session["typeBac"] = userLogin.prenom;
                     Session["anneeBac"] = userLogin.anneeBac;
                     Session["noteBac"] = userLogin.noteBac;
                     Session["mentionBac"] = userLogin.mentionBac;
                     Session["noteFstYear"] = userLogin.noteFstYear;
                     Session["noteSndYear"] = userLogin.noteSndYear;
                     Session["dateNaiss"] = userLogin.dateNaiss;
                     Session["lieuNaiss"] = userLogin.lieuNaiss;
                     Session["photo_link"] = userLogin.photo_link;
                     Session["choix"] = userLogin.choix;*/
                    Session["role"] = "Etudiant";
                    return RedirectToAction("Index", "Etudiant");
                }
                else if (userLogin == null)
                {

                    message = "Invalid cin or cne or password";
                    ViewBag.Message = message;
                    return View();
                }

            }
        return View();

        }

    }
   
}
