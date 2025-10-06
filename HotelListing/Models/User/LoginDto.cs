using HotelListing.Data;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models.User
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set;  }
        public ApiUser User { get; set; }
    }
}
