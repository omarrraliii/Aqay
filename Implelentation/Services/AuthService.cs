using aqay_apis.Helpers;
using aqay_apis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace aqay_apis.Services
{
    public class AuthService : IAuthService
    {
        // Service attributes
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AuthService(RoleManager<IdentityRole> roleManager,IOptions<JWT> jwt, UserManager<User> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }


        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            // check if it exists already
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                authModel.Message = "No such an Email found!";
                return authModel;
            }
            // check the passwords
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                authModel.Message = "Wrong Password! try again if u own the account and stop hacking people if you don't!";
                return authModel;
            }

            // Create a User Token to Login
            var jwtSecurityToken = await CreateJwtToken(user);

            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.UserName = user.UserName;

            // Create a list for the user's roles then assign it to the auth model
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            return authModel;
        }

        public async Task<AuthModel> SignupConsumerAsync(SignupConsumerModel model)
        {
            // first check if there is a consumer with the same email
            if (await  _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered!" };
            }

            // Extract username from email
            string[] emailParts = model.Email.Split('@');
            string userName = emailParts[0];

            // if it doesn't exsist then create a new merchant 
            var consumer = new Consumer
            {
                Email = model.Email,
                UserName = userName,
                Gender = model.Gender,
                Day = model.Day,
                Month = model.Month,
                Year = model.Year,
            };
            // register the new consumer in the db
            var result = await  _userManager.CreateAsync(consumer, model.Password);

            // if the password and the password Confirm didn't match
            if(model.Password != model.PasswordConfirm)
            {
                var error = "Passwords Don't Match!";
                return new AuthModel { Message = error };
            }

            // print all errors if any
            if (!result.Succeeded)
            {
                var errors = " ";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return new AuthModel { Message = errors };

            }

            // add the user to a role consumer Automatically
            await  _userManager.AddToRoleAsync(consumer, "Consumer");


            // Create a User Token to Login after SignUp
            var jwtSecurityToken = await CreateJwtToken(consumer);

            return new AuthModel
            {
                Email = consumer.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Consumer" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = userName
            };

        }

        public async Task<AuthModel> SignupMerchantAsync(SignupMerchantModel model)
        {
            // first check if there is a merchant with the same email
            if (await  _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered!" };
            }
            // Extract username from email
            string[] emailParts = model.Email.Split('@');
            string userName = emailParts[0];

            // if it doesn't exsist then create a new merchant 
            var merchant = new Merchant
            {
                Email = model.Email,
                UserName = userName,
                IsOwner = true,
                IsVerified = true,// temporarily --->> to be changed!
            };

            // if the password and the password Confirm didn't match
            if (model.Password != model.PasswordConfirm)
            {
                var error = "Passwords Don't Match!";
                return new AuthModel { Message = error };
            }
            // register the new merchant in the db
            var result = await _userManager.CreateAsync(merchant, model.Password);


            // print all errors if any
            if (!result.Succeeded)
            {
                var errors = " ";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return new AuthModel { Message = errors };

            }

            // add the user to a role consumer Automatically
            await  _userManager.AddToRoleAsync(merchant, "Owner");


            // Create a User Token to Login after SignUp
            var jwtSecurityToken = await CreateJwtToken(merchant);

            return new AuthModel
            {
                Email = merchant.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Owner" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = userName
            };
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
