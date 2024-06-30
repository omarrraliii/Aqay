using aqay_apis;
using aqay_apis.Helpers;
using aqay_apis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Http;
using static aqay_apis.Helpers.Enums;

namespace aqay_apis.Services
{
    public class AuthService : IAuthService
    {
        // Service attributes
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IMailingService _mailingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public AuthService(RoleManager<IdentityRole> roleManager,IOptions<JWT> jwt, IMailingService mailingService, 
            UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, LinkGenerator
            linkGenerator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mailingService = mailingService;
            _jwt = jwt.Value;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
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

            /*
             var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(consumer);
            var request = _httpContextAccessor.HttpContext.Request;
            var confirmationLink = _linkGenerator.GetUriByAction(
                httpContext: _httpContextAccessor.HttpContext,
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId = consumer.Id, token = confirmationToken },
                scheme: request.Scheme);

            await _mailingService.SendEmailAsync(consumer.Email,
                "Please Confirm your Email",
                $"Please click on this link to confirm your email address: <a href='{confirmationLink}'>Confirm Email</a>");
            */

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

            // if it doesn't exist then create the following:

            // 1) New Subscription, still pending, will be approved upon payment completion

            var newSubscription = new Subscription
            {
                PlanId = 1,
                Status = SubscriptionStatus.PENDING
            };

            // 2) A merchant is created
            
            var merchant = new Merchant
            {
                Email = model.Email,
                UserName = userName,
                IsOwner = true,
                IsVerified = true,// temporarily --->> to be changed!
                //SubscriptionId = 1,
            };

            // 3) Attaching the new subscription created to the new merchant

            merchant.Subscription = newSubscription;





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
            /*
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(merchant);
            var request = _httpContextAccessor.HttpContext.Request;
            var confirmationLink = _linkGenerator.GetUriByAction(
                httpContext: _httpContextAccessor.HttpContext,
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId = merchant.Id, token = confirmationToken },
                scheme: request.Scheme);

            await _mailingService.SendEmailAsync(merchant.Email,
                "Please Confirm your Email",
                $"Please click on this link to confirm your email address: <a href='{confirmationLink}'>Confirm Email</a>");
            // add the user to a role consumer Automatically
            await  _userManager.AddToRoleAsync(merchant, "Owner");
            */

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
