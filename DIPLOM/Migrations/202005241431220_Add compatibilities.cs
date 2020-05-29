namespace DIPLOM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcompatibilities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutoParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Article = c.String(),
                        Name = c.String(),
                        GroupName = c.String(),
                        SubGroupName = c.String(),
                        Price = c.Double(nullable: false),
                        Proportions = c.String(),
                        Weight = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Compatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AutoPart_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AutoParts", t => t.AutoPart_Id)
                .Index(t => t.AutoPart_Id);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mark = c.String(),
                        Model = c.String(),
                        SelectedAutoPart_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SelectedAutoParts", t => t.SelectedAutoPart_Id)
                .Index(t => t.SelectedAutoPart_Id);
            
            CreateTable(
                "dbo.Complectations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SelectedAutoParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Complectation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Complectations", t => t.Complectation_Id)
                .Index(t => t.Complectation_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Client = c.String(),
                        Price = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        PassHash = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SelectedAutoParts", "Complectation_Id", "dbo.Complectations");
            DropForeignKey("dbo.Cars", "SelectedAutoPart_Id", "dbo.SelectedAutoParts");
            DropForeignKey("dbo.Compatibilities", "AutoPart_Id", "dbo.AutoParts");
            DropIndex("dbo.SelectedAutoParts", new[] { "Complectation_Id" });
            DropIndex("dbo.Cars", new[] { "SelectedAutoPart_Id" });
            DropIndex("dbo.Compatibilities", new[] { "AutoPart_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Producers");
            DropTable("dbo.Orders");
            DropTable("dbo.Groups");
            DropTable("dbo.SelectedAutoParts");
            DropTable("dbo.Complectations");
            DropTable("dbo.Cars");
            DropTable("dbo.Compatibilities");
            DropTable("dbo.AutoParts");
        }
    }
}
