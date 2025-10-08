using AutoMapper;
using HotelListing.Contracts.User;
using HotelListing.Data;
using HotelListing.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<IAuthRepository> _logger;
        private readonly IConfiguration _configuraton;
        public AuthRepository(IMapper mapper, UserManager<ApiUser> userManager, ILogger<IAuthRepository> logger,IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _configuraton = configuration;



        }

        

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

        public async Task<LoginResponseDto?> Login(LoginDto loginDto)
        {
            bool valid = false;
            var user = await  _userManager.FindByEmailAsync(loginDto.Email);
            Console.WriteLine(user);
            if (user != null)
            {
            valid=  await   _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (valid)
                {
                    var userUpt= _mapper.Map<UserResponseDto>(user);                
                    return new LoginResponseDto
                    {
                        Token = await GetToken(user),
                        User = userUpt

                    };
                }
            }
            return null;






           


        }
        public async Task<string> GetToken(ApiUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
           
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuraton["JwtSettings:Key"]));


            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var userClaims = await _userManager.GetClaimsAsync(user);
            Console.WriteLine(userClaims);
         var roleClaims = roles.Select(r=> new Claim (ClaimTypes.Role,r)).ToList();
            //create  claims,add user details and create a union of role ,user and your claims
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email ),
              new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Name,user.LastName),
                new Claim ("uid", user.Id)

            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuraton["JwtSettings:Issuer"],
                audience: _configuraton["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuraton["JwtSettings:Duration"])),
                signingCredentials: credentials


                );
            _logger.LogInformation("not fomratted {@token} ", token);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
