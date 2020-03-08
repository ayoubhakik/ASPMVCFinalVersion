namespace projetASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departements",
                c => new
                    {
                        id_departement = c.Int(nullable: false, identity: true),
                        nom_departement = c.String(),
                        email = c.String(),
                        username = c.String(),
                        password = c.String(),
                        phone = c.String(),
                    })
                .PrimaryKey(t => t.id_departement);
            
            CreateTable(
                "dbo.Etudiants",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nom = c.String(),
                        prenom = c.String(),
                        password = c.String(),
                        nationalite = c.String(),
                        cin = c.String(),
                        cne = c.String(),
                        email = c.String(),
                        phone = c.String(),
                        gsm = c.String(),
                        address = c.String(),
                        ville = c.String(),
                        typeBac = c.String(),
                        anneeBac = c.Int(nullable: false),
                        noteBac = c.Double(nullable: false),
                        mentionBac = c.String(),
                        noteFstYear = c.Double(nullable: false),
                        noteSndYear = c.Double(nullable: false),
                        dateNaiss = c.DateTime(nullable: false),
                        photo_link = c.String(),
                        choix = c.String(),
                        validated = c.Boolean(nullable: false),
                        modified = c.Boolean(nullable: false),
                        idFil = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Filieres", t => t.idFil)
                .Index(t => t.idFil);
            
            CreateTable(
                "dbo.Filieres",
                c => new
                    {
                        idFil = c.Int(nullable: false, identity: true),
                        nomFil = c.String(),
                    })
                .PrimaryKey(t => t.idFil);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Etudiants", "idFil", "dbo.Filieres");
            DropIndex("dbo.Etudiants", new[] { "idFil" });
            DropTable("dbo.Filieres");
            DropTable("dbo.Etudiants");
            DropTable("dbo.Departements");
        }
    }
}
