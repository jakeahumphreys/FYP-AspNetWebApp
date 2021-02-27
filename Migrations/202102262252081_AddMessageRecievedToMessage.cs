namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageRecievedToMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MessageRecieved", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "MessageRecieved");
        }
    }
}
