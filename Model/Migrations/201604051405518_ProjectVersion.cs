namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Version", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Version");
        }
    }
}
