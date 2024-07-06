using aqay_apis.Models;
using aqay_apis.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using aqay_apis.Models;
namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // inject the service
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
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

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.ResetPasswordAsync(model.Email, model.OldPassword, model.NewPassword, model.dateOfBirth);
                return Ok(new
                {
                    Message = "Password reset successful."
                });
            }
            catch (Exception ex)
            {
                // Log the exception with details (excluding sensitive data)
                return BadRequest($"Error resetting password for {model.Email}: {ex.Message}");
            }
        }
    }
}
