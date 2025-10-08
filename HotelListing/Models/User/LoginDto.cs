using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public UserResponseDto User { get; set; }
    }
    public class UserResponseDto:ApiUser
    {
        [JsonIgnore]
        public override string PasswordHash { get; set; }
        public override string ConcurrencyStamp { get; set; }
        public override string SecurityStamp { get; set; }
    }
}
