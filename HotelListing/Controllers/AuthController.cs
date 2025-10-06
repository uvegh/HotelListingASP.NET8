using HotelListing.Contracts.User;
using HotelListing.Models.User;
using HotelListing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{

    [ApiController]
    [Route("hotelApi/[controller]")]
    public class AuthController : ControllerBase

    {
        private readonly IAuthRepository _authRepo;



        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;

        }

        //post signup
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityError>> Signup([FromBody] SignupDto signupDto)
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


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<bool>> Login([FromBody]LoginDto loginDto
            )
        {
            var res = await _authRepo.Login(loginDto);

            if (res!=null)
            {
                return Ok(res);
            }

            return Unauthorized("invalid Login details");



        }
    }
}
