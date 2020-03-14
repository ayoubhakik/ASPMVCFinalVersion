namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Etudiants", "confirmPass", c => c.String());
            AddColumn("dbo.Etudiants", "confirmCin", c => c.String());
            AddColumn("dbo.Etudiants", "confirmEmail", c => c.String());
            AddColumn("dbo.Etudiants", "lieuNaiss", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Etudiants", "lieuNaiss");
            DropColumn("dbo.Etudiants", "confirmEmail");
            DropColumn("dbo.Etudiants", "confirmCin");
            DropColumn("dbo.Etudiants", "confirmPass");
        }
    }
}
