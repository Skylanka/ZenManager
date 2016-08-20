namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issues", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issues", "Priority");
        }
    }
}
