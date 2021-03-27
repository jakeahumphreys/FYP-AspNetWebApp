namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeLabelRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StoredLocations", "Label", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StoredLocations", "Label", c => c.String());
        }
    }
}
