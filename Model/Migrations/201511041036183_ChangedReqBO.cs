namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedReqBO : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Requirements", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requirements", "Type", c => c.Int(nullable: false));
        }
    }
}
