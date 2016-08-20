namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDocs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documentation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        ProjectBO_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectBO_Id)
                .Index(t => t.ProjectBO_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documentation", "ProjectBO_Id", "dbo.Projects");
            DropIndex("dbo.Documentation", new[] { "ProjectBO_Id" });
            DropTable("dbo.Documentation");
        }
    }
}
