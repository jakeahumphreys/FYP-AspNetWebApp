namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccessLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Ip = c.String(),
                        AttemptedUser = c.String(),
                        Success = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccessLogs");
        }
    }
}
