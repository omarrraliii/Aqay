using aqay_apis.Context;
using aqay_apis.Helpers;
using aqay_apis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        private readonly IWishListService _wishListService;
        private readonly IMailingService _mailingService;
        public AuthService(RoleManager<IdentityRole> roleManager,IMailingService mailingService,IWishListService wishListService ,IOptions<JWT> jwt, UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
            _wishListService = wishListService;
            _mailingService = mailingService;
        }
        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                authModel.Message = "No such email found!";
                return authModel;
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                authModel.Message = "Incorrect password!";
                return authModel;
            }

            if (!user.IsActive)
            {
                authModel.Message = "User is inactive. Cannot log in.";
                return authModel;
            }

            // Check if the user is a merchant and is subscribed
            if (await _userManager.IsInRoleAsync(user, "Merchant"))
            {
                var merchant = await _context.Merchants.FindAsync(user.Id);
                if (merchant == null) {
                    authModel.Message = "Merchant not found. Cannot log in.";
                    return authModel;

                }
                if (!merchant.IsSubscriped)
                {
                    authModel.Message = "User is a merchant but not subscribed. Cannot log in.";
                    return authModel;
                }
                authModel.isSubscriped = false;
            }
            else
            {
                authModel.isSubscriped = true;
            }

            var jwtSecurityToken = await CreateJwtToken(user);

            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.UserName = user.UserName;
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
            // if the password and the password Confirm didn't match
            if (model.Password != model.PasswordConfirm)
            {
                var error = "Passwords Don't Match!";
                return new AuthModel { Message = error };
            }

            // register the new consumer in the db
            var result = await _userManager.CreateAsync(consumer, model.Password);

            // add the user to a role consumer Automatically
            await  _userManager.AddToRoleAsync(consumer, "Consumer");

            // create a wishlist and link it with the consumer
            var wishList = await _wishListService.CreateWishListAsync(consumer);
            consumer.WishList = wishList;
            consumer.WishListId = wishList.Id;
            await _userManager.UpdateAsync(consumer);
            await _context.SaveChangesAsync();

            // Create a User Token to Login after SignUp
            var jwtSecurityToken = await CreateJwtToken(consumer);

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

            return new AuthModel
            {
                Email = consumer.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Consumer" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = userName,
                isSubscriped = true
            };

        }
        public async Task<string> SignupMerchantAsync(SignupMerchantModel model)
        {
            bool IsValid = false;
            bool TRN = false;
            bool NAT = false;
            // first check if there is a merchant with the same email
            if (await  _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return "Email is already registered!";
            }
            // check if one of the IDs is provided ( only one)
            if (model.NationalId == null && model.TaxRegistrationNumber == null)
            {
                return "National ID or Tax Registration Number Must be provided";
            }
            // National Id is provided 
            else if (!string.IsNullOrEmpty(model.NationalId) && string.IsNullOrEmpty(model.TaxRegistrationNumber)) {
                IsValid = NationalIDValidator.ValidateEGYNationalID(model.NationalId);
                NAT = true;
            }
            // Tax num is provided 
            else if (string.IsNullOrEmpty(model.NationalId) && !string.IsNullOrEmpty(model.TaxRegistrationNumber))
            {
                IsValid = TRNValidator.ValidateTaxRegistrationNumber(model.TaxRegistrationNumber);
                TRN = true;
            }
            else
            {
                return "Only one is needed";
            }
            if (!IsValid)
            {
                return "non Valid!";
            }
            if (NAT)
            {
                // send data to consumer
                var info = NationalIDValidator.ExtractFormattedInfo(model.NationalId);
                await _mailingService.SendEmailAsync(model.Email, "Data check", info, null);
            }
            // Extract username from email
            string[] emailParts = model.Email.Split('@');
            string userName = emailParts[0];

            // pending merchant 
            PendingMerchant pendingMerchant = new PendingMerchant
            {
                Password = model.Password,
                Email = model.Email,
                Username = userName,
                NATID = (NAT)? model.NationalId : null,
                TRN = (TRN)? model.TaxRegistrationNumber : null
            };

            // if the password and the password Confirm didn't match
            if (model.Password != model.PasswordConfirm)
            {
                return "Passwords Don't Match!";
            }
            // register the new pending merchant in the db
            _context.PendingMerchants.Add(pendingMerchant);
            await _context.SaveChangesAsync();
            return "keep an eye on Your Email .. your account is being validated";

        }

        //Create a user token
        public async Task<JwtSecurityToken> CreateJwtToken(User user)
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

        public async Task<string> CreateAdminAsync(string email, string password)
        {
            if (await _userManager.FindByEmailAsync(email) is not null)
            {
                return "Email is already registered!";
            }

            string[] emailParts = email.Split('@');
            string userName = emailParts[0];

            var adminUser = new User
            {
                Email = email,
                UserName = userName
            };

            var result = await _userManager.CreateAsync(adminUser, password);

            if (!result.Succeeded)
            {
                var errors = " ";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return errors;
            }

            await _userManager.AddToRoleAsync(adminUser, "Admin");

            return "Admin created successfully!";
        }
    }
}
