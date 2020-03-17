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
                        cne = c.String(nullable: false, maxLength: 128),
                        nom = c.String(nullable: false, maxLength: 10),
                        prenom = c.String(nullable: false, maxLength: 10),
                        password = c.String(nullable: false),
                        nationalite = c.String(nullable: false),
                        cin = c.String(nullable: false),
                        email = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        gsm = c.String(nullable: false),
                        address = c.String(nullable: false),
                        ville = c.String(nullable: false),
                        typeBac = c.String(nullable: false),
                        anneeBac = c.Int(nullable: false),
                        noteBac = c.Double(nullable: false),
                        mentionBac = c.String(nullable: false),
                        noteFstYear = c.Double(nullable: false),
                        noteSndYear = c.Double(nullable: false),
                        dateNaiss = c.DateTime(nullable: false),
                        lieuNaiss = c.String(nullable: false),
                        photo_link = c.String(nullable: false),
                        choix = c.String(),
                        Validated = c.Boolean(nullable: false),
                        Modified = c.Boolean(nullable: false),
                        idFil = c.Int(),
                    })
                .PrimaryKey(t => t.cne)
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
