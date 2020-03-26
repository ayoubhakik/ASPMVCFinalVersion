namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etudiants", "dateNaiss", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Etudiants", "dateNaiss", c => c.DateTime(nullable: false));
        }
    }
}
