using AutoMapper;
using HotelListing.Contracts.User;
using HotelListing.Data;
using HotelListing.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<IAuthRepository> _logger;
        public AuthRepository(IMapper mapper, UserManager<ApiUser> userManager, ILogger<IAuthRepository> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;


        }

        //    public async Task<SignupResultDto> Signup(SignupDto signupDto)
        //    {
        //        var user = _mapper.Map<ApiUser>(signupDto);
        //        user.UserName = signupDto.Email;
        //        var res = await _userManager.CreateAsync(user, signupDto.Password);
        //        _logger.LogInformation("signup response {@res}", res);
        //        if (res.Succeeded)
        //        {
        //            await _userManager.AddToRoleAsync(user, "user");

        //            return  new SignupResultDto { User=user};
        //        }
        //        return  new SignupResultDto { Errors=res.Errors};


        //    }
        //}
        

        public async Task<IEnumerable<IdentityError>> Signup(SignupDto signupDto)
        {
            var user = _mapper.Map<ApiUser>(signupDto);
            user.UserName = signupDto.Email;

            var res = await _userManager.CreateAsync(user, signupDto.Password);
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");

            }
          
            return res.Errors;

        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            var  valid = false;

            try {

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user != null)
                {
                    valid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                    return valid;
                }
                _logger.LogInformation("user data retrieved {user}", user);
                _logger.LogInformation("user data retrieved {valid}", valid);
                return valid;

               
            }

            catch (Exception) {
                throw;
            
            }
           


        }
    }

}
