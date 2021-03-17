namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addApiKeyToConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigurationRecords", "MapsApiKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConfigurationRecords", "MapsApiKey");
        }
    }
}
