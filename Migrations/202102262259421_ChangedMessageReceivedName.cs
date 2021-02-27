namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedMessageReceivedName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MessageReceived", c => c.DateTime(nullable: false));
            DropColumn("dbo.Messages", "MessageRecieved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "MessageRecieved", c => c.DateTime(nullable: false));
            DropColumn("dbo.Messages", "MessageReceived");
        }
    }
}
