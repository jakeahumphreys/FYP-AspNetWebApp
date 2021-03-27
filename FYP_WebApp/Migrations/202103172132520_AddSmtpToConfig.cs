namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSmtpToConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigurationRecords", "SmtpSendUrgentEmails", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfigurationRecords", "SmtpUrl", c => c.String());
            AddColumn("dbo.ConfigurationRecords", "SmtpPort", c => c.Int(nullable: false));
            AddColumn("dbo.ConfigurationRecords", "SmtpEmailFrom", c => c.String());
            AddColumn("dbo.ConfigurationRecords", "SmtpSenderUsername", c => c.String());
            AddColumn("dbo.ConfigurationRecords", "SmtpSenderPassword", c => c.String());
            AddColumn("dbo.ConfigurationRecords", "SmtpShouldUseSsl", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfigurationRecords", "SmtpRecipientEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConfigurationRecords", "SmtpRecipientEmail");
            DropColumn("dbo.ConfigurationRecords", "SmtpShouldUseSsl");
            DropColumn("dbo.ConfigurationRecords", "SmtpSenderPassword");
            DropColumn("dbo.ConfigurationRecords", "SmtpSenderUsername");
            DropColumn("dbo.ConfigurationRecords", "SmtpEmailFrom");
            DropColumn("dbo.ConfigurationRecords", "SmtpPort");
            DropColumn("dbo.ConfigurationRecords", "SmtpUrl");
            DropColumn("dbo.ConfigurationRecords", "SmtpSendUrgentEmails");
        }
    }
}
