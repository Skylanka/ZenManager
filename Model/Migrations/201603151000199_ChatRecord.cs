namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Time = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Project_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatRecords", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ChatRecords", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ChatRecords", new[] { "User_Id" });
            DropIndex("dbo.ChatRecords", new[] { "Project_Id" });
            DropTable("dbo.ChatRecords");
        }
    }
}
