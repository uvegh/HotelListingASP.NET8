using HotelListing.Core.Entities;
using HotelListing.Data.Configurations;
using HotelListing.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class HotelDBContext:IdentityDbContext<ApiUser>
    {
        public HotelDBContext(DbContextOptions<HotelDBContext> options ):base(options) { 
        
        }

      public DbSet<Hotel> Hotels { get; set; }
      public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Country>();
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
