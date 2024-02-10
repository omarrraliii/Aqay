using Aqay_v2.Models;
using Aqay_v2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Aqay_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> SignupAsync([FromBody]SignupModel model)
        {
            //check that model is valid
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.SignupAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            //If authenticated that indicates that the user is successfully created
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            //check that model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("Supscripe")]
        public async Task<IActionResult> SupscripeAsync([FromBody] SupscriptionModel model)
        {
            //check that model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.SupscripeAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }
            return Ok(model);
        }
    }
}
