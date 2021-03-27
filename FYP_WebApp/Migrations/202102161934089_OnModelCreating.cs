namespace FYP_WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnModelCreating : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GpsReports", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.GpsReports", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.StoredLocations", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.StoredLocations", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StoredLocations", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.StoredLocations", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GpsReports", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GpsReports", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
