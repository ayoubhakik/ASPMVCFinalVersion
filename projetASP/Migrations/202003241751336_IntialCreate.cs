namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        id_Config = c.Int(nullable: false, identity: true),
                        Delai = c.DateTime(nullable: false),
                        importEtudiant = c.Boolean(nullable: false),
                        importNote = c.Boolean(nullable: false),
                        DatedeRappel = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_Config);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Configs");
        }
    }
}
