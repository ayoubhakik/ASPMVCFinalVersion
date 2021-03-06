﻿namespace projetASP.Migrations
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
                        cne = c.String(nullable: false, maxLength: 10),
                        nom = c.String(nullable: false),
                        prenom = c.String(nullable: false),
                        password = c.String(),
                        nationalite = c.String(),
                        cin = c.String(nullable: false, maxLength: 8),
                        email = c.String(),
                        phone = c.String(maxLength: 10),
                        gsm = c.String(maxLength: 10),
                        address = c.String(),
                        ville = c.String(),
                        typeBac = c.String(),
                        anneeBac = c.Int(nullable: false),
                        noteBac = c.Double(nullable: false),
                        mentionBac = c.String(),
                        noteFstYear = c.Double(nullable: false),
                        noteSndYear = c.Double(nullable: false),
                        dateNaiss = c.DateTime(),
                        lieuNaiss = c.String(),
                        photo_link = c.String(),
                        Choix = c.String(),
                        Validated = c.Boolean(nullable: false),
                        Modified = c.Boolean(nullable: false),
                        Redoubler = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        idSettings = c.Int(nullable: false, identity: true),
                        Delai = c.DateTime(nullable: false),
                        importEtudiant = c.Boolean(nullable: false),
                        importNote = c.Boolean(nullable: false),
                        Attributted = c.Boolean(nullable: false),
                        DatedeRappel = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.idSettings);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Etudiants", "idFil", "dbo.Filieres");
            DropIndex("dbo.Etudiants", new[] { "idFil" });
            DropTable("dbo.Settings");
            DropTable("dbo.Filieres");
            DropTable("dbo.Etudiants");
            DropTable("dbo.Departements");
        }
    }
}
