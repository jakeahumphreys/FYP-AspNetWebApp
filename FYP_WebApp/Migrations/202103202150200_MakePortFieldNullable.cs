namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePortFieldNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConfigurationRecords", "SmtpPort", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConfigurationRecords", "SmtpPort", c => c.Int(nullable: false));
        }
    }
}
