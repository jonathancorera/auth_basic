using AuthTest.Dtos.Auth;
using AuthTest.Dtos.Users;
using AuthTest.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (LoginRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try {
                var result = await _authService.Login(user);

                if (result != null)
                {


   


                    LoginResponseDto responseDto= new LoginResponseDto()
                    {
                        Id = result.Id,
                       Email = result.Email,
                       Jwt = _authService.GenerateTokenString(result),
                       FirstName = result.FirstName,
                       LastName = result.LastName,

                       
                    };

                    return Ok(responseDto);
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUser(UserCreationDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try {
                var result = await _authService.RegisterUser(user);

                if (result)
                {

                    return Ok("success");
                }
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
