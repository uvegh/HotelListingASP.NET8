using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Data.Configurations
{
    public class HotelConfiguration:IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(

                
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
                    Id = 2,
                    Address = "Faldon Lodge ",
                    CountryId = 2,
                    PostCode = "B1,8BQ",
                    Name = "Falcon Central Hotels",
                    Rating = 4
                }
                
                );


        }
    }
}
