namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsIssues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.Int(),
                        Requirement_Id = c.Int(),
                        UpdatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Requirements", t => t.Requirement_Id)
                .ForeignKey("dbo.Users", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Requirement_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "dbo.Issues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Details = c.String(),
                        Status = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        AssignedTo_Id = c.Int(),
                        CreatedBy_Id = c.Int(),
                        UpdatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssignedTo_Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Users", t => t.UpdatedBy_Id)
                .Index(t => t.AssignedTo_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            AddColumn("dbo.Requirements", "IssueBO_Id", c => c.Int());
            CreateIndex("dbo.Requirements", "IssueBO_Id");
            AddForeignKey("dbo.Requirements", "IssueBO_Id", "dbo.Issues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Issues", "UpdatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Requirements", "IssueBO_Id", "dbo.Issues");
            DropForeignKey("dbo.Issues", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Issues", "AssignedTo_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "UpdatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "Requirement_Id", "dbo.Requirements");
            DropForeignKey("dbo.Comments", "CreatedBy_Id", "dbo.Users");
            DropIndex("dbo.Issues", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Issues", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Issues", new[] { "AssignedTo_Id" });
            DropIndex("dbo.Comments", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Comments", new[] { "Requirement_Id" });
            DropIndex("dbo.Comments", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Requirements", new[] { "IssueBO_Id" });
            DropColumn("dbo.Requirements", "IssueBO_Id");
            DropTable("dbo.Issues");
            DropTable("dbo.Comments");
        }
    }
}
