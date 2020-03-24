namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate1 : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        id_Config = c.Int(nullable: false, identity: true),
                        Delai = c.DateTime(nullable: false),
                        importEtudiant = c.Boolean(nullable: false),
                        importNote = c.Boolean(nullable: false),
                        Attributted = c.Boolean(nullable: false),
                        DatedeRappel = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_Config);
            
        }
    }
}
