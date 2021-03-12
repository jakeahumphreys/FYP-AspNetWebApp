namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIpFromAccessLog : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AccessLogs", "Ip");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccessLogs", "Ip", c => c.String());
        }
    }
}
