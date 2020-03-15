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
        
        public ActionResult Authentification()
        {
            String button = Request["loginBtn"];
            if (button == "Login")
            {
                string userName = Request["userName"];
                string mdp= Request["mdp"];
                EtudiantContext dbSet = new EtudiantContext();
                var userLogin = (from data in dbSet.departements
                                 where data.username == userName && data.password == mdp select data).FirstOrDefault();
                if (userLogin != null)
                {
                    Session["userName"] =userLogin.username;
                    Session["NomDep"] = userLogin.nom_departement;
                    Session["EmailDep"] = userLogin.email;
                    Session["userId"] = userLogin.id_departement;
                    return RedirectToAction("Home", "Departement");
                }
                else if (userLogin == null)
                {
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