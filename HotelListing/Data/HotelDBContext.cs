using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class HotelDBContext:DbContext
    {
        public HotelDBContext(DbContextOptions<HotelDBContext> options ):base(options) { 
        
        }

      public DbSet<Hotel> Hotels { get; set; }
      public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name="Nigeria",
                    ShortName="NG"
                },
                new Country
                {
                    Id=2,
                    Name="United Kingdom",
                    ShortName="UK"
                },
                new Country
                {
                    Id=3,
                    Name="United States of America",
                    ShortName="USA"
                }
              
                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Address = "Lagos,Banana Island",
                    CountryId = 1,
                    Name = "Eko hotel",
                    Rating = 3.5
                },
                new Hotel
                {
                    Id=2,
                    Address="Faldon Lodge ",
                    CountryId=2,
                    PostCode="B1,8BQ",
                    Name="Falcon Central Hotels",
                    Rating=4
                }
                );
        }
    }
}
