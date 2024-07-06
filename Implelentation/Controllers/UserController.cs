using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using aqay_apis.Models;
using aqay_apis.Dtos;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.FlowAnalysis;
using aqay_apis.Context;
namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IBrandService _brandService;
        public UserController(IBrandService brandService,UserManager<User> userManager,ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _context = applicationDbContext;
            _brandService = brandService;
        }
        // GET api/user/brand-id?id={userId}
        [HttpGet("brand-id")]
        public async Task<IActionResult> GetBrandIdAsync([FromQuery] string id)
        {
            // Find the user by Id
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the user is of type Merchant (assuming Merchant inherits from User)
            if (user is Merchant merchant)
            {
                // Return the BrandId of the merchant
                var brandName = merchant.BrandName;
                var brandId = _brandService.GetBrandIdBy(brandName);
                return Ok(new { BrandId = brandId });
            }
            else
            {
                // If the user is not of type Merchant, return an error or handle accordingly
                return BadRequest("User is not a Merchant.");
            }
        }
        // GET api/users/{id}/profile
        [HttpGet("/profile")]
            public async Task<ActionResult<UserProfileDto>> GetUserProfile(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var userProfile = new UserProfileDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth
                };

                return Ok(userProfile);
            }

            // PUT api/users/{id}/profile
            [HttpPut("/profile")]
            public async Task<IActionResult> UpdateUserProfile([FromQuery]string id, UpdateUserDto updateUserDto)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.Gender = updateUserDto.Gender;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return NoContent(); // Successfully updated
            }
        }
    }