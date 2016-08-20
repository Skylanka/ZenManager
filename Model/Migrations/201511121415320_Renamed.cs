namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renamed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttributesTypes", "InternalValue", c => c.String());
            DropColumn("dbo.AttributesTypes", "Values");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttributesTypes", "Values", c => c.String());
            DropColumn("dbo.AttributesTypes", "InternalValue");
        }
    }
}
