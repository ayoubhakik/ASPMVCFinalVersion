namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<projetASP.DAL.EtudiantContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(projetASP.DAL.EtudiantContext context)
        {
            context.settings.Add(new Models.Settings
            {
                Attributted = false, importEtudiant = false, importNote = false, Delai = Convert.ToDateTime("10/10/2040"),
                DatedeRappel = Convert.ToDateTime("10/10/2040")
            });
            context.Filieres.Add(new Models.Filiere
            {
                nomFil = "informatique"
            });
            context.Filieres.Add(new Models.Filiere
            {
                nomFil = "Indus"
            });
            context.Filieres.Add(new Models.Filiere
            {
                nomFil = "gtr"
            });
            context.Filieres.Add(new Models.Filiere
            {
                nomFil = "gpmc"
            });
            context.departements.Add(new Models.Departement
            {
                password = "root",
                username = "root",
                nom_departement = "Testeur de projet"
            }
                );
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
