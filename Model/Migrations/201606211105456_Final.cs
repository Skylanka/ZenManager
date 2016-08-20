namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UpdatedBy_Id", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UpdatedBy_Id" });
            RenameColumn(table: "dbo.Projects", name: "Author_Id", newName: "CreatedBy_Id");
            RenameColumn(table: "dbo.Requirements", name: "Author_Id", newName: "CreatedBy_Id");
            RenameIndex(table: "dbo.Projects", name: "IX_Author_Id", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "dbo.Requirements", name: "IX_Author_Id", newName: "IX_CreatedBy_Id");
            AddColumn("dbo.Projects", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "UpdatedBy_Id", c => c.Int());
            AddColumn("dbo.Requirements", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Issues", "Description", c => c.String());
            CreateIndex("dbo.Projects", "UpdatedBy_Id");
            AddForeignKey("dbo.Projects", "UpdatedBy_Id", "dbo.Users", "Id");
            DropColumn("dbo.Projects", "CreatedDate");
            DropColumn("dbo.Requirements", "CreatedDate");
            DropColumn("dbo.Attributes", "CreatedDate");
            DropColumn("dbo.Comments", "Updated");
            DropColumn("dbo.Comments", "UpdatedBy_Id");
            DropColumn("dbo.Issues", "Details");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Issues", "Details", c => c.String());
            AddColumn("dbo.Comments", "UpdatedBy_Id", c => c.Int());
            AddColumn("dbo.Comments", "Updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Attributes", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requirements", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "CreatedDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Projects", "UpdatedBy_Id", "dbo.Users");
            DropIndex("dbo.Projects", new[] { "UpdatedBy_Id" });
            DropColumn("dbo.Issues", "Description");
            DropColumn("dbo.Requirements", "Created");
            DropColumn("dbo.Projects", "UpdatedBy_Id");
            DropColumn("dbo.Projects", "Created");
            RenameIndex(table: "dbo.Requirements", name: "IX_CreatedBy_Id", newName: "IX_Author_Id");
            RenameIndex(table: "dbo.Projects", name: "IX_CreatedBy_Id", newName: "IX_Author_Id");
            RenameColumn(table: "dbo.Requirements", name: "CreatedBy_Id", newName: "Author_Id");
            RenameColumn(table: "dbo.Projects", name: "CreatedBy_Id", newName: "Author_Id");
            CreateIndex("dbo.Comments", "UpdatedBy_Id");
            AddForeignKey("dbo.Comments", "UpdatedBy_Id", "dbo.Users", "Id");
        }
    }
}
