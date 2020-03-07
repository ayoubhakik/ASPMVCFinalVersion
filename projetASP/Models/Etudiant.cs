using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projetASP.Models
{
    public class Etudiant
    {
        public int id;
        public string nom;
        public string prenom;
        public string email;
        public string phone;
        public string photo_link;
        public int age;
        public double note;
        public string password;
        public Boolean validated;
        public Boolean modified;

    }
}