namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReqChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requirements", "Updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requirements", "UpdatedBy_Id", c => c.Int());
            CreateIndex("dbo.Requirements", "UpdatedBy_Id");
            AddForeignKey("dbo.Requirements", "UpdatedBy_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requirements", "UpdatedBy_Id", "dbo.Users");
            DropIndex("dbo.Requirements", new[] { "UpdatedBy_Id" });
            DropColumn("dbo.Requirements", "UpdatedBy_Id");
            DropColumn("dbo.Requirements", "Updated");
        }
    }
}
