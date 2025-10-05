using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models.User
{
    public class SignupDto:BaseUserDto
    {
       

        [PasswordPropertyText]
        public string Password { get; set; }
    }

    public class SignupResultDto
    {
        public IEnumerable<IdentityError> Errors= Enumerable.Empty<IdentityError>();

        public ApiUser? User  { get; set; }

        public bool Success => !Errors.Any() && User != null;
    }
}
