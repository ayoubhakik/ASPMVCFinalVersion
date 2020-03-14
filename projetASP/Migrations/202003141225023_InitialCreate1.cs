namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Etudiants", "lieuNaiss", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Etudiants", "lieuNaiss");
        }
    }
}
