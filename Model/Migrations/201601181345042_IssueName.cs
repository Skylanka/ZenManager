namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IssueName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issues", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issues", "Name");
        }
    }
}
