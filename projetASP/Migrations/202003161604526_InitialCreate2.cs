namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etudiants", "photo_link", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Etudiants", "photo_link", c => c.String(nullable: false));
        }
    }
}
