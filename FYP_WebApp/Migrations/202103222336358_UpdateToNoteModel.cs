namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToNoteModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Notes", new[] { "SenderId" });
            AlterColumn("dbo.Notes", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Notes", "SenderId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Notes", "SenderId");
            AddForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Notes", new[] { "SenderId" });
            AlterColumn("dbo.Notes", "SenderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Notes", "Content", c => c.String());
            CreateIndex("dbo.Notes", "SenderId");
            AddForeignKey("dbo.Notes", "SenderId", "dbo.AspNetUsers", "Id");
        }
    }
}
