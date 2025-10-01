using HotelListing.Models.Country;

namespace HotelListing.Models.Hotel
{
    public class BaseHotelDto
    {

        
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Rating { get; set; }
        public int CountryId { get; set; }
       
        public string? PostCode { get; set; }
    }
}
