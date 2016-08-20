namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPRA : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescription_Id", "dbo.AttributesTypes");
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescription_Id", "dbo.RequirementsTypes");
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "RequirementsDescription_Id" });
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "AttributeDescription_Id" });
            RenameColumn(table: "dbo.ProjectsRequirenmentAttributeTypes", name: "AttributeDescription_Id", newName: "AttributeDescriptionId");
            RenameColumn(table: "dbo.ProjectsRequirenmentAttributeTypes", name: "RequirementsDescription_Id", newName: "RequirementsDescriptionId");
            DropPrimaryKey("dbo.ProjectsRequirenmentAttributeTypes");
            AlterColumn("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescriptionId", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescriptionId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProjectsRequirenmentAttributeTypes", new[] { "ProjectId", "RequirementsDescriptionId", "AttributeDescriptionId" });
            CreateIndex("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescriptionId");
            CreateIndex("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescriptionId");
            AddForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescriptionId", "dbo.AttributesTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescriptionId", "dbo.RequirementsTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.ProjectsRequirenmentAttributeTypes", "RequirenmentTypeId");
            DropColumn("dbo.ProjectsRequirenmentAttributeTypes", "AttributeTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectsRequirenmentAttributeTypes", "AttributeTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectsRequirenmentAttributeTypes", "RequirenmentTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescriptionId", "dbo.RequirementsTypes");
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescriptionId", "dbo.AttributesTypes");
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "AttributeDescriptionId" });
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "RequirementsDescriptionId" });
            DropPrimaryKey("dbo.ProjectsRequirenmentAttributeTypes");
            AlterColumn("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescriptionId", c => c.Int());
            AlterColumn("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescriptionId", c => c.Int());
            AddPrimaryKey("dbo.ProjectsRequirenmentAttributeTypes", new[] { "ProjectId", "RequirenmentTypeId", "AttributeTypeId" });
            RenameColumn(table: "dbo.ProjectsRequirenmentAttributeTypes", name: "RequirementsDescriptionId", newName: "RequirementsDescription_Id");
            RenameColumn(table: "dbo.ProjectsRequirenmentAttributeTypes", name: "AttributeDescriptionId", newName: "AttributeDescription_Id");
            CreateIndex("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescription_Id");
            CreateIndex("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescription_Id");
            AddForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescription_Id", "dbo.RequirementsTypes", "Id");
            AddForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescription_Id", "dbo.AttributesTypes", "Id");
        }
    }
}
