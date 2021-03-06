namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMobileLoginKeuy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MobileLoginKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MobileLoginKey");
        }
    }
}
