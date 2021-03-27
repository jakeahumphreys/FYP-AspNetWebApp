namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnforceFieldsOnTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Teams", new[] { "ManagerId" });
            AlterColumn("dbo.Teams", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Teams", "ManagerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Teams", "ManagerId");
            AddForeignKey("dbo.Teams", "ManagerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Teams", new[] { "ManagerId" });
            AlterColumn("dbo.Teams", "ManagerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Teams", "Name", c => c.String());
            CreateIndex("dbo.Teams", "ManagerId");
            AddForeignKey("dbo.Teams", "ManagerId", "dbo.AspNetUsers", "Id");
        }
    }
}
