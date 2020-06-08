namespace DIPLOM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutoParts", "Group_Id", c => c.Int());
            CreateIndex("dbo.AutoParts", "Group_Id");
            AddForeignKey("dbo.AutoParts", "Group_Id", "dbo.Groups", "Id");
            DropColumn("dbo.AutoParts", "GroupName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AutoParts", "GroupName", c => c.String());
            DropForeignKey("dbo.AutoParts", "Group_Id", "dbo.Groups");
            DropIndex("dbo.AutoParts", new[] { "Group_Id" });
            DropColumn("dbo.AutoParts", "Group_Id");
        }
    }
}
