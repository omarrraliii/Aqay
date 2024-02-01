using Aqay_v2.Models;
using Microsoft.AspNetCore.Identity;

namespace Aqay_v2.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AuthModel> SignupAsync(SignupModel model)
        {
            //check if the user already exists
            if(await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = " Email is already regiestered" };
            }
            if (await _userManager.FindByNameAsync (model.Username) is not null)
            {
                return new AuthModel { Message = " Username is already regiestered" };
            }
            //create a new user to add in the database
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname
            };
            //If there was any error return it as a message
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return new AuthModel { Message = errors };
            }

        }
    }
}
