using HotelListing.Models.Country;

namespace HotelListing.Models.Hotel
{
    public sealed class HotelDto:BaseHotelDto
    {
        public int Id { get; set; }
        public GetCountryDto Country { get; set; }


    }
}
