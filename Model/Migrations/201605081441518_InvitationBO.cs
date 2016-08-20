namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvitationBO : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Guid(nullable: false),
                        Email = c.String(),
                        Status = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "ApplicationUser_Id", "dbo.Users");
            DropIndex("dbo.Invitations", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Invitations");
        }
    }
}
