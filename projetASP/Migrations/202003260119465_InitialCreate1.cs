namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etudiants", "phone", c => c.String(maxLength: 10));
            AlterColumn("dbo.Etudiants", "gsm", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Etudiants", "gsm", c => c.String());
            AlterColumn("dbo.Etudiants", "phone", c => c.String());
        }
    }
}
