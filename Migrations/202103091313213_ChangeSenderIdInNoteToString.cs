namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSenderIdInNoteToString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notes", new[] { "Sender_Id" });
            DropColumn("dbo.Notes", "SenderId");
            RenameColumn(table: "dbo.Notes", name: "Sender_Id", newName: "SenderId");
            AlterColumn("dbo.Notes", "SenderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notes", "SenderId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Notes", new[] { "SenderId" });
            AlterColumn("dbo.Notes", "SenderId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Notes", name: "SenderId", newName: "Sender_Id");
            AddColumn("dbo.Notes", "SenderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Notes", "Sender_Id");
        }
    }
}
