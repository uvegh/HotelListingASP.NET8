using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelListing.Models.User
{
    public class AuthDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set;  }
        public string RefreshToken { get; set; }
        public UserResponseDto User { get; set; }
    }

    public class RefreshRequestDto 
    {
        public string UserId { get; set; }
        
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

    }

    public class UserResponseDto:ApiUser
    {
        [JsonIgnore]
        public override string PasswordHash { get; set; }
        public override string ConcurrencyStamp { get; set; }
        public override string SecurityStamp { get; set; }
    }
}
