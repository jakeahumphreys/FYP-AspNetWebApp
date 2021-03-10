namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRouteFieldsToApiLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApiLogs", "Controller", c => c.String());
            AddColumn("dbo.ApiLogs", "Action", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApiLogs", "Action");
            DropColumn("dbo.ApiLogs", "Controller");
        }
    }
}
