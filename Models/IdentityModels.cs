using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP_WebApp.Common_Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FYP_WebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        //Information Fields
        [Display(Name = "First Name(s)")]
        public string FirstName { get; set; }
        [Display(Name = "Surname(s)")]
        public string Surname { get; set; }

        //Gps Fields
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        //Admin Fields
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public bool IsInactive { get; set; }

        //Identity override fields
        [Display(Name = "Account Locked Until")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        override public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Account can be locked")]
        override public bool LockoutEnabled { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoredLocation>().Property(x => x.Latitude).HasPrecision(18, 9);
            modelBuilder.Entity<StoredLocation>().Property(x => x.Longitude).HasPrecision(18, 9);
            modelBuilder.Entity<GpsReport>().Property(x => x.Latitude).HasPrecision(18, 9);
            modelBuilder.Entity<GpsReport>().Property(x => x.Longitude).HasPrecision(18, 9);

            base.OnModelCreating(modelBuilder);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ConfigurationRecord> ConfigurationRecords { get; set; }
        public DbSet<GpsReport> GpsReports { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Pairing> Pairings { get; set; }
        public DbSet<StoredLocation> StoredLocations { get; set; }
        public DbSet<Team> Teams { get; set; }

    }
}