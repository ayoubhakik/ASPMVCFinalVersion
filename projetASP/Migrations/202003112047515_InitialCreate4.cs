namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Etudiants", "confirmPass");
            DropColumn("dbo.Etudiants", "confirmCin");
            DropColumn("dbo.Etudiants", "confirmEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Etudiants", "confirmEmail", c => c.String(nullable: false));
            AddColumn("dbo.Etudiants", "confirmCin", c => c.String(nullable: false));
            AddColumn("dbo.Etudiants", "confirmPass", c => c.String(nullable: false));
        }
    }
}
