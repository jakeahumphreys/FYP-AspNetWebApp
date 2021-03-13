namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectGpsAndLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GpsReports", "LocationId", c => c.Int());
            CreateIndex("dbo.GpsReports", "LocationId");
            AddForeignKey("dbo.GpsReports", "LocationId", "dbo.StoredLocations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GpsReports", "LocationId", "dbo.StoredLocations");
            DropIndex("dbo.GpsReports", new[] { "LocationId" });
            DropColumn("dbo.GpsReports", "LocationId");
        }
    }
}
