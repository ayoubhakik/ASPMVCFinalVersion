namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Etudiants");
            AlterColumn("dbo.Etudiants", "cne", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Etudiants", "cne");
            DropColumn("dbo.Etudiants", "id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Etudiants", "id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Etudiants");
            AlterColumn("dbo.Etudiants", "cne", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Etudiants", "id");
        }
    }
}
