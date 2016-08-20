namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentChanged : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "IssueBO_Id", newName: "Issue_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_IssueBO_Id", newName: "IX_Issue_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Comments", name: "IX_Issue_Id", newName: "IX_IssueBO_Id");
            RenameColumn(table: "dbo.Comments", name: "Issue_Id", newName: "IssueBO_Id");
        }
    }
}
