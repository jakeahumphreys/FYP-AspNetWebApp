namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManagerToTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "TeamId", "dbo.Teams");
            AddColumn("dbo.AspNetUsers", "Team_Id", c => c.Int());
            AddColumn("dbo.Teams", "ManagerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Team_Id");
            CreateIndex("dbo.Teams", "ManagerId");
            AddForeignKey("dbo.Teams", "ManagerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Team_Id", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Teams", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Teams", new[] { "ManagerId" });
            DropIndex("dbo.AspNetUsers", new[] { "Team_Id" });
            DropColumn("dbo.Teams", "ManagerId");
            DropColumn("dbo.AspNetUsers", "Team_Id");
            AddForeignKey("dbo.AspNetUsers", "TeamId", "dbo.Teams", "Id");
        }
    }
}
