namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeMasterRecipientEmail : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ConfigurationRecords", "SmtpRecipientEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConfigurationRecords", "SmtpRecipientEmail", c => c.String());
        }
    }
}
