namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRoleDesc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "Description");
        }
    }
}
