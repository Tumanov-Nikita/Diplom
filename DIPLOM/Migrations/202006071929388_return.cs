namespace DIPLOM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _return : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AutoParts", "Group_Id", "dbo.Groups");
            DropIndex("dbo.AutoParts", new[] { "Group_Id" });
            AddColumn("dbo.AutoParts", "GroupName", c => c.String());
            DropColumn("dbo.AutoParts", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AutoParts", "Group_Id", c => c.Int());
            DropColumn("dbo.AutoParts", "GroupName");
            CreateIndex("dbo.AutoParts", "Group_Id");
            AddForeignKey("dbo.AutoParts", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
