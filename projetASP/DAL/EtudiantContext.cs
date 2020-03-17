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

        public DbSet<Etudiant> etudiants { get; set; }
        public DbSet<Departement> departements { get; set; }

        public System.Data.Entity.DbSet<projetASP.Models.Filiere> Filieres { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EtudiantContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
    

}