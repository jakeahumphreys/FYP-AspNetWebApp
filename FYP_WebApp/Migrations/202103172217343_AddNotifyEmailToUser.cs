namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifyEmailToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NotifyEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NotifyEmail");
        }
    }
}
