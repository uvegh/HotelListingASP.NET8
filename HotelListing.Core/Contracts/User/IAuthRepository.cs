
using HotelListing.Models.User;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Contracts.User
{
    public interface IAuthRepository
    {

        Task <IEnumerable<IdentityError>> Signup(SignupDto signupDto);
        Task<LoginResponseDto?> Login(AuthDto loginDto);
        Task<string> GetAccessToken();
        Task<bool> UpdateUserRole(EmailDto email);
        Task<LoginResponseDto> VerifyRefreshToken(RefreshRequestDto refreshDto);
        Task<string> GetRefreshToken();
    }
}
