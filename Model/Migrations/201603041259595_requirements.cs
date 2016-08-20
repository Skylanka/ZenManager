namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requirements : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirements", "RequirementBO_Id", "dbo.Requirements");
            DropIndex("dbo.Requirements", new[] { "RequirementBO_Id" });
            CreateTable(
                "dbo.ReqLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        RequirementFrom_Id = c.Int(),
                        RequirementTo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requirements", t => t.RequirementFrom_Id)
                .ForeignKey("dbo.Requirements", t => t.RequirementTo_Id)
                .Index(t => t.RequirementFrom_Id)
                .Index(t => t.RequirementTo_Id);
            
            AddColumn("dbo.Comments", "IssueBO_Id", c => c.Int());
            CreateIndex("dbo.Comments", "IssueBO_Id");
            AddForeignKey("dbo.Comments", "IssueBO_Id", "dbo.Issues", "Id");
            DropColumn("dbo.Requirements", "RequirementBO_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requirements", "RequirementBO_Id", c => c.Int());
            DropForeignKey("dbo.ReqLinks", "RequirementTo_Id", "dbo.Requirements");
            DropForeignKey("dbo.ReqLinks", "RequirementFrom_Id", "dbo.Requirements");
            DropForeignKey("dbo.Comments", "IssueBO_Id", "dbo.Issues");
            DropIndex("dbo.ReqLinks", new[] { "RequirementTo_Id" });
            DropIndex("dbo.ReqLinks", new[] { "RequirementFrom_Id" });
            DropIndex("dbo.Comments", new[] { "IssueBO_Id" });
            DropColumn("dbo.Comments", "IssueBO_Id");
            DropTable("dbo.ReqLinks");
            CreateIndex("dbo.Requirements", "RequirementBO_Id");
            AddForeignKey("dbo.Requirements", "RequirementBO_Id", "dbo.Requirements", "Id");
        }
    }
}
