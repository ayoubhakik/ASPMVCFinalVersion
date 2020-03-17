using projetASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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

        public DbSet<Filiere> Filieres { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }
    }
}