namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etudiants", "nom", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "prenom", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Etudiants", "prenom", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Etudiants", "nom", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
