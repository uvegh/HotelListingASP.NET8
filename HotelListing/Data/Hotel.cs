using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Data
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        [ForeignKey(nameof (CountryId))]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [MaxLength(9,ErrorMessage ="Post code cannot be more than 9 characters")]
        public string? PostCode { get; set; }


    }
}
