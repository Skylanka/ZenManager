namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvitationBOToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "ProjectBO_Id", c => c.Int());
            CreateIndex("dbo.Invitations", "ProjectBO_Id");
            AddForeignKey("dbo.Invitations", "ProjectBO_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "ProjectBO_Id", "dbo.Projects");
            DropIndex("dbo.Invitations", new[] { "ProjectBO_Id" });
            DropColumn("dbo.Invitations", "ProjectBO_Id");
        }
    }
}
