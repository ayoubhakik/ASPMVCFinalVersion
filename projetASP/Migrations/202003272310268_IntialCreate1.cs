namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Etudiants", "choix");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Etudiants", "choix", c => c.String());
        }
    }
}
