using HotelListing.Contracts.User;
using HotelListing.Exceptions;
using HotelListing.Models.User;
using HotelListing.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Controllers
{

    [ApiController]
    [Route("hotelApi/[controller]")]
    public class AuthController : ControllerBase

    {
        private readonly IAuthRepository _authRepo;
        private readonly ILogger _logger;


        public AuthController(IAuthRepository authRepo, ILogger<AuthController> logger)
        {
            _authRepo = authRepo;
            _logger = logger;

        }

        //post signup
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityError>> Signup([FromBody] SignupDto signupDto)
        {
            try
            {
                var res = await _authRepo.Signup(signupDto);
                if (res.Any())
                {
                    foreach (var err in res)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok(signupDto);

            }
            catch (Exception )
            {
                _logger.LogDebug($" something went wrong in the {nameof(Signup)} controller");

                return Problem($"Something went wrong in the {nameof(Signup)} controller");
            }
           




        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<bool>> Login([FromBody] AuthDto loginDto
            )
        {
            var res = await _authRepo.Login(loginDto);
            Console.WriteLine(res);
            if (res != null)
            {
                return Ok(res);
            }

             throw  new UnAuthorizedException (nameof(Login));



        }
        [Authorize]
        [HttpPost]
        [Route("updateRole")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateUserRole([FromBodyAttribute] EmailDto email)
        {
            bool res = await _authRepo.UpdateUserRole(email);
            if (!res) return StatusCode(StatusCodes.Status403Forbidden);
            return Ok();

        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<LoginResponseDto>> GetRefreshTOken([FromBody] RefreshRequestDto refreshDto)
        {
            Console.WriteLine(refreshDto);
            var res = await _authRepo.VerifyRefreshToken(refreshDto);
            
            if (res == null) return StatusCode(StatusCodes.Status401Unauthorized);
            return Ok(res);
        }
    }
}
