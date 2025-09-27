using HotelListing.Models.Country;

namespace HotelListing.Models.Hotel
{
    public sealed class HotelDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } 
        public double Rating { get; set; }
        public int CountryId { get; set; }  
        public GetCountryDto Country { get; set; }
        public string? PostCode { get; set; }
    }
}
