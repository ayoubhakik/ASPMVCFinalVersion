namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Etudiants", "lieuNaiss");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Etudiants", "lieuNaiss", c => c.String(nullable: false));
        }
    }
}
