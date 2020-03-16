namespace projetASP.Migrations
{
    using projetASP.Models;
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

            //var etudiants = new List<Models.Etudiant>();
            //etudiants.Add(new Models.Etudiant());
            //etudiants.Add(new Models.Etudiant());
            //etudiants.Add(new Models.Etudiant());


           

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
