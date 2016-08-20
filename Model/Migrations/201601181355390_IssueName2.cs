namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IssueName2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issues", "Project_Id", c => c.Int());
            CreateIndex("dbo.Issues", "Project_Id");
            AddForeignKey("dbo.Issues", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Issues", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Issues", new[] { "Project_Id" });
            DropColumn("dbo.Issues", "Project_Id");
        }
    }
}
