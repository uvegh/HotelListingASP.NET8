using HotelListing.Data;
using HotelListing.Models.Hotel;

namespace HotelListing.Models.Country
{
    public class GetCountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string shortName { get; set; }
        //public IList<Hotel> Hotels { get; set; }

    }

    public class CountryDto:BaseCountryDto
    {
        public int Id { get; set; }
        

        public virtual List<HotelDto> Hotels { get; set; }

    }

    public class  UpdateCountryDto:BaseCountryDto
    {
        public int Id { get; set; }
        
    }
}
