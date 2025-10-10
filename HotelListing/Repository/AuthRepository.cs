using AutoMapper;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using HotelListing.Contracts.User;
using HotelListing.Data;
using HotelListing.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IAuthRepository> _logger;
        private readonly IConfiguration _configuraton;
        private ApiUser _user;
        private readonly string _loginProvider = "HotelListing";
        private readonly string _refreshToken = "RefreshToken";
        public AuthRepository(IMapper mapper, UserManager<ApiUser> userManager, ILogger<IAuthRepository> logger,IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _configuraton = configuration;
            _roleManager = roleManager;



        }

        

        public async Task<IEnumerable<IdentityError>> Signup(SignupDto signupDto)
        {
             _user = _mapper.Map<ApiUser>(signupDto);
            _user.UserName = signupDto.Email;

            var res = await _userManager.CreateAsync(_user, signupDto.Password);
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "user");

            }
          
            return res.Errors;

        }

        public async Task<string> GetRefreshToken()
        {

            //delete current tokens params(user,loginprovider=hotlellisitng,token name=refresh token)
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            //generate new token
            var refreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            //setnewtoken
             await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, refreshToken);
            return refreshToken;
        }
        public async Task<LoginResponseDto?>VerifyRefreshToken(RefreshRequestDto refreshDto)
        {
            //_logger.LogInformation(" this is token {@AccessToken}", refreshDto.AccessToken);
            //read expired access token to get claims
            var jwtHandler = new JwtSecurityTokenHandler();
            var userObj = jwtHandler.ReadJwtToken(refreshDto.AccessToken);
            //_logger.LogInformation(" this is user obj {@userObj}", userObj);

            //claims that were created when setting up accesstoken, retutn token type subject(Sub) value
            var email = userObj.Claims.ToList().FirstOrDefault(res => res.Type == JwtRegisteredClaimNames.Sub)?.Value;
            //var id= userObj.Claims.ToList()
            //_logger.LogInformation( "this is sub -{@email}", email);
            if (email != null ) {
               
                //get user with claim
            _user=    await _userManager.FindByNameAsync(email);
                _logger.LogInformation("_user id before the if ==null statement {@Id}", _user.Id);
                _logger.LogInformation("Id from request body id before the if ==null statement {@UserId}", refreshDto.UserId);
                if (_user != null && _user.Id==refreshDto.UserId)
                {
                    _logger.LogInformation(" this is userId--{@UserId}", refreshDto.UserId);
                    _logger.LogInformation("user info {@_user.Id}", _user.Id);
                    _logger.LogInformation("user info after the if null statement {@_user}", _user);
                    //verify token,pass user, provider,tokentype,
                    var isRefreshValid=await _userManager.VerifyUserTokenAsync(_user,  _loginProvider,_refreshToken,refreshDto.RefreshToken);
                    if (isRefreshValid)
                    {
                        return new LoginResponseDto
                        {
                            RefreshToken = await GetRefreshToken(),
                            AccessToken = await GetAccessToken(),
                            User = _mapper.Map<UserResponseDto>(_user)
                        };

                    }
                    await _userManager.UpdateSecurityStampAsync(_user);
                    return null;
                }
                return null;
            }
            return null;
        }

        public async Task<LoginResponseDto?> Login(AuthDto loginDto)
        {
            bool valid = false;
             _user = await  _userManager.FindByEmailAsync(loginDto.Email);
            Console.WriteLine(_user);
            if (_user != null)
            {
            valid=  await   _userManager.CheckPasswordAsync(_user, loginDto.Password);

                if (valid)
                {
                    var userUpt= _mapper.Map<UserResponseDto>(_user);
                    return new LoginResponseDto
                    {
                        RefreshToken = await GetRefreshToken(),

                        AccessToken = await GetAccessToken(),
                        User = userUpt

                    };
                }
            }
            return null;






           


        }
        public async Task<string> GetAccessToken()
        {
            var roles = await _userManager.GetRolesAsync(_user);
           
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuraton["JwtSettings:Key"]));


            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var userClaims = await _userManager.GetClaimsAsync(_user);
            Console.WriteLine(userClaims);
         var roleClaims = roles.Select(r=> new Claim (ClaimTypes.Role,r)).ToList();
            //create  claims,add user details and create a union of role ,user and your claims
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, _user.Email ),
              new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Name,_user.LastName),
                new Claim ("uid", _user.Id)

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
        public async Task<bool> UpdateUserRole(EmailDto Email)
        {
            var email = Email.Email;
            _logger.LogCritical("{@email} user email", email);
            bool didUpdate = false;
            var user =await  _userManager.FindByEmailAsync(email);
            if (user == null) return didUpdate ;
            _logger.LogCritical("{@user} user", user);
            var roleExist = await _roleManager.RoleExistsAsync("Administrator");
            await _userManager.RemoveFromRoleAsync(user,"User");
            await _userManager.AddToRoleAsync(user, "Administrator");
            var updatedRole = await _userManager.GetRolesAsync(user);
            _logger.LogCritical("{@updatedRole} updated role", updatedRole);
            didUpdate = updatedRole.Contains("Administrator");
            _logger.LogCritical("{@updatedRole} did update? ", didUpdate);
            //if (updatedRole.Contains("Administrator"){
            //    return true;
            //}
            _logger.LogCritical("{@roleExist} check role", roleExist);
            return didUpdate;
          
            
         


        }
    }

}
