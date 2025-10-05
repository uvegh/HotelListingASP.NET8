using HotelListing.Data;
using HotelListing.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Contracts.User
{
    public interface IAuthRepository
    {

        Task <IEnumerable<IdentityError>> Signup(SignupDto signupDto);
        Task<bool> Login(LoginDto loginDto);
    }
}
