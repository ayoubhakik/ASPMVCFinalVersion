using projetASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace projetASP.DAL
{
    public class EtudiantContext : DbContext
    {
        public EtudiantContext() : base("EtudiantContext")
        {

        }
        public DbSet<Etudiant> etudiants { get; set; }
        public DbSet<Departement> departements { get; set; }

        public System.Data.Entity.DbSet<projetASP.Models.Filiere> Filieres { get; set; }
    }
}