namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdditionalFieldsToNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "SenderId", c => c.Int(nullable: false));
            AddColumn("dbo.Notes", "TimeCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notes", "Sender_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notes", "Sender_Id");
            AddForeignKey("dbo.Notes", "Sender_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notes", new[] { "Sender_Id" });
            DropColumn("dbo.Notes", "Sender_Id");
            DropColumn("dbo.Notes", "TimeCreated");
            DropColumn("dbo.Notes", "SenderId");
        }
    }
}
