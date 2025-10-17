
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using HotelListing.Contracts.User;
using HotelListing.Data.Entities;


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
