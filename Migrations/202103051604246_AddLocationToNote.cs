namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationToNote : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "StoredLocation_Id", "dbo.StoredLocations");
            DropIndex("dbo.Notes", new[] { "StoredLocation_Id" });
            RenameColumn(table: "dbo.Notes", name: "StoredLocation_Id", newName: "StoredLocationId");
            AlterColumn("dbo.Notes", "StoredLocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Notes", "StoredLocationId");
            AddForeignKey("dbo.Notes", "StoredLocationId", "dbo.StoredLocations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "StoredLocationId", "dbo.StoredLocations");
            DropIndex("dbo.Notes", new[] { "StoredLocationId" });
            AlterColumn("dbo.Notes", "StoredLocationId", c => c.Int());
            RenameColumn(table: "dbo.Notes", name: "StoredLocationId", newName: "StoredLocation_Id");
            CreateIndex("dbo.Notes", "StoredLocation_Id");
            AddForeignKey("dbo.Notes", "StoredLocation_Id", "dbo.StoredLocations", "Id");
        }
    }
}
