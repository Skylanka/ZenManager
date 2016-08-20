namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Status");
            DropColumn("dbo.Projects", "Updated");
        }
    }
}
