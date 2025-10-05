using HotelListing.Data;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models.User
{
    public class BaseUserDto
    {

        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
