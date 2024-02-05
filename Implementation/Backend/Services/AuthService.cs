using Aqay_v2.Helpers;
using Aqay_v2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aqay_v2.Services
{
    public class AuthService : IAuthService
    {
        //Service Attributes
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        //Constructor
        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }
        public async Task<AuthModel> SignupAsync(SignupModel model)
        {
            // Check if the user already exists
            if (await _userManager.FindByNameAsync(model.Username) is not null)
            {
                return new AuthModel { Message = "Username is already registered" };
            }

            // Check if the email has reached the maximum number of allowed accounts (2)
            var existingAccounts = await _userManager.FindByEmailAsync(model.Email);
            if (existingAccounts != null)
            {
                var existingRoles = await _userManager.GetRolesAsync(existingAccounts);
                if (existingRoles.Count >= 2)
                {
                    return new AuthModel { Message = "Maximum number of accounts reached for this email" };
                }
            }

            // Create a new user to add to the database
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname
            };
            // If there was any error, return it as a message
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return new AuthModel { Message = errors };
            }
            // use subscripe as option
            // Create user as a buyer by default
            await _userManager.AddToRoleAsync(user, "Buyer");

            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authmodel = new AuthModel();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user is null)
            {
                authmodel.Message = "Username or Password is incorrect!\n";
                return authmodel;
            }

            // Check if the user has exceeded the maximum allowed password attempts
            var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            if (accessFailedCount >= 3)
            {
                // Send a warning email to the user
                SendWarningEmail(user.Email);
                authmodel.Message = "You have exceeded the maximum allowed password attempts. An email has been sent to you for security purposes. Please watch out!\n";
                return authmodel;
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authmodel.Message = "Password is incorrect. Please try again!\n";

                // Increment access failed count since the password attempt was unsuccessful
                await _userManager.AccessFailedAsync(user);

                return authmodel;
            }

            // If the password is correct, reset access failed count
            await _userManager.ResetAccessFailedCountAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            authmodel.IsAuthenticated = true;
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authmodel.Email = user.Email;
            authmodel.UserName = user.UserName;
            authmodel.ExpiresOn = jwtSecurityToken.ValidTo;
            authmodel.Roles = rolesList.ToList();
            return authmodel;
        }
        public async Task<string> SupscripeAsync(SupscriptionModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
            {
                return "Invalid User Id\n";   
            }
            if(!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return "Invalid Role\n";
            }
            if(await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return $"User is already assigned to{model.RoleName}\n";
            }
            var result = await _userManager.AddToRoleAsync(user,model.RoleName);
            if (result.Succeeded)
            {
                return "Done!\n";
            }
            else
            {
                return "Something went wrong and i don't know what it is\n figure it out on your own :(";
            }
        }
        private void SendWarningEmail(string userEmail)
        {
            // to Implement the logic to send a warning email to the user 
        }

        //Create a user token
        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            //get rules and claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
