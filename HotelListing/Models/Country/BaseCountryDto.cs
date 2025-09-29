using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models.Country
{
    public class BaseCountryDto
    {


        [Required]
        public string Name { get; set; }
        public string shortName { get; set; }
    }
}
