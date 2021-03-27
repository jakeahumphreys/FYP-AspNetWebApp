namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageToAccessLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessLogs", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessLogs", "Message");
        }
    }
}
