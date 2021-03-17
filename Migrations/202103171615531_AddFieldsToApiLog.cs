namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToApiLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApiLogs", "AdditionalFields", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApiLogs", "AdditionalFields");
        }
    }
}
