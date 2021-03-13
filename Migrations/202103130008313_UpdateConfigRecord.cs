namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateConfigRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigurationRecords", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.ConfigurationRecords", "StoreLocationAccuracy", c => c.Double(nullable: false));
            AddColumn("dbo.ConfigurationRecords", "MessageOfTheDayText", c => c.String());
            DropColumn("dbo.ConfigurationRecords", "Name");
            DropColumn("dbo.ConfigurationRecords", "IsInactive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConfigurationRecords", "IsInactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfigurationRecords", "Name", c => c.Int(nullable: false));
            DropColumn("dbo.ConfigurationRecords", "MessageOfTheDayText");
            DropColumn("dbo.ConfigurationRecords", "StoreLocationAccuracy");
            DropColumn("dbo.ConfigurationRecords", "Created");
        }
    }
}
