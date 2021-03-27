namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTitleFromNote : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notes", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notes", "Title", c => c.String());
        }
    }
}
