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
    }
   
}