using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using aqay_apis.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.FlowAnalysis;
namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // PUT api/user/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromQuery]string id, [FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user by Id
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user properties
            user.PhoneNumber = (request.PhoneNumber == null || request.PhoneNumber == " ") ? user.PhoneNumber : request.PhoneNumber;
            user.Gender = (request.Gender == null) ? user.Gender = user.Gender : request.Gender;

            // Update user in the database
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Return a response indicating success
                var updateResponse = new UpdateResponse
                {
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender
                };
                return Ok(updateResponse);
            }
            else
            {
                // Return any validation errors or failure reasons
                return BadRequest(result.Errors);
            }
        }
    }
}


public class UpdateProfileRequest
{
    public string PhoneNumber { get; set; }
    public bool Gender { get; set; }
}

public class UpdateResponse
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool Gender { get; set; }
}
