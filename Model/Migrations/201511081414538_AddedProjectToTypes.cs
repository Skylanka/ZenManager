namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectToTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttributesTypes", "Project_Id", c => c.Int());
            AddColumn("dbo.RequirementsTypes", "Project_Id", c => c.Int());
            CreateIndex("dbo.AttributesTypes", "Project_Id");
            CreateIndex("dbo.RequirementsTypes", "Project_Id");
            AddForeignKey("dbo.RequirementsTypes", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.AttributesTypes", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AttributesTypes", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.RequirementsTypes", "Project_Id", "dbo.Projects");
            DropIndex("dbo.RequirementsTypes", new[] { "Project_Id" });
            DropIndex("dbo.AttributesTypes", new[] { "Project_Id" });
            DropColumn("dbo.RequirementsTypes", "Project_Id");
            DropColumn("dbo.AttributesTypes", "Project_Id");
        }
    }
}
