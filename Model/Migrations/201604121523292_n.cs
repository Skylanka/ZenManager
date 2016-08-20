namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class n : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirements", "IssueBO_Id", "dbo.Issues");
            DropIndex("dbo.Requirements", new[] { "IssueBO_Id" });
            CreateTable(
                "dbo.IssueBORequirementBOes",
                c => new
                    {
                        IssueBO_Id = c.Int(nullable: false),
                        RequirementBO_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IssueBO_Id, t.RequirementBO_Id })
                .ForeignKey("dbo.Issues", t => t.IssueBO_Id, cascadeDelete: true)
                .ForeignKey("dbo.Requirements", t => t.RequirementBO_Id, cascadeDelete: true)
                .Index(t => t.IssueBO_Id)
                .Index(t => t.RequirementBO_Id);
            
            DropColumn("dbo.Requirements", "IssueBO_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requirements", "IssueBO_Id", c => c.Int());
            DropForeignKey("dbo.IssueBORequirementBOes", "RequirementBO_Id", "dbo.Requirements");
            DropForeignKey("dbo.IssueBORequirementBOes", "IssueBO_Id", "dbo.Issues");
            DropIndex("dbo.IssueBORequirementBOes", new[] { "RequirementBO_Id" });
            DropIndex("dbo.IssueBORequirementBOes", new[] { "IssueBO_Id" });
            DropTable("dbo.IssueBORequirementBOes");
            CreateIndex("dbo.Requirements", "IssueBO_Id");
            AddForeignKey("dbo.Requirements", "IssueBO_Id", "dbo.Issues", "Id");
        }
    }
}
