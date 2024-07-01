using aqay_apis.Models;
using aqay_apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // inject the service
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        // POST ENDPOINT: Sign up as a consumer 
        [HttpPost("SignupConsumer")]
        public async Task<IActionResult> SignupConsumerAsync([FromBody]SignupConsumerModel model)
        {
            // check if the model is valid
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result  = await _authService.SignupConsumerAsync(model);

            // check if the model is authenticated
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        // POST ENDPOINT: Sign up as a merchant 
        [HttpPost("SignupMerchant")]
        public async Task<IActionResult> SignupMerchantAsync([FromBody] SignupMerchantModel model)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.SignupMerchantAsync(model);
            return Ok(result);
        }

        // POST ENDPOINT: Login for both
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(model);

            // check if the model is aut henticated
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        // POST ENDPOINT: Create an admin
        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdminAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.CreateAdminAsync(model.Email, model.Password);

            if (result != "Admin created successfully!")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
