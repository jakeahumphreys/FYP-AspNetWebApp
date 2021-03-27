namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToNoteModelRemoveRequirements : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Notes", new[] { "SenderId" });
            AlterColumn("dbo.Notes", "SenderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notes", "SenderId");
            AddForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Notes", new[] { "SenderId" });
            AlterColumn("dbo.Notes", "SenderId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Notes", "SenderId");
            AddForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
