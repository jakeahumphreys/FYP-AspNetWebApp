namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApiLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogLevel = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        RequestString = c.String(),
                        ResponseString = c.String(),
                        StatusCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApiLogs");
        }
    }
}
