namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etudiants", "nom", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Etudiants", "prenom", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Etudiants", "password", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "confirmPass", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "nationalite", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "cin", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "confirmCin", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "cne", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "email", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "confirmEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "phone", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "gsm", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "address", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "ville", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "typeBac", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "mentionBac", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "lieuNaiss", c => c.String(nullable: false));
            AlterColumn("dbo.Etudiants", "photo_link", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Etudiants", "photo_link", c => c.String());
            AlterColumn("dbo.Etudiants", "lieuNaiss", c => c.String());
            AlterColumn("dbo.Etudiants", "mentionBac", c => c.String());
            AlterColumn("dbo.Etudiants", "typeBac", c => c.String());
            AlterColumn("dbo.Etudiants", "ville", c => c.String());
            AlterColumn("dbo.Etudiants", "address", c => c.String());
            AlterColumn("dbo.Etudiants", "gsm", c => c.String());
            AlterColumn("dbo.Etudiants", "phone", c => c.String());
            AlterColumn("dbo.Etudiants", "confirmEmail", c => c.String());
            AlterColumn("dbo.Etudiants", "email", c => c.String());
            AlterColumn("dbo.Etudiants", "cne", c => c.String());
            AlterColumn("dbo.Etudiants", "confirmCin", c => c.String());
            AlterColumn("dbo.Etudiants", "cin", c => c.String());
            AlterColumn("dbo.Etudiants", "nationalite", c => c.String());
            AlterColumn("dbo.Etudiants", "confirmPass", c => c.String());
            AlterColumn("dbo.Etudiants", "password", c => c.String());
            AlterColumn("dbo.Etudiants", "prenom", c => c.String());
            AlterColumn("dbo.Etudiants", "nom", c => c.String());
        }
    }
}
