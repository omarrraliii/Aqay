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
        private readonly IShoppingCartService _shoppingCartService;
        public AuthService(RoleManager<IdentityRole> roleManager,IShoppingCartService shoppingCartService,IMailingService mailingService,IWishListService wishListService ,IOptions<JWT> jwt, UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
            _wishListService = wishListService;
            _mailingService = mailingService;
            _shoppingCartService = shoppingCartService;
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
            if (await _userManager.IsInRoleAsync(user, "Owner"))
            {
                var merchant = await _context.Merchants.FindAsync(user.Id);
                if (merchant == null) {
                    authModel.Message = "Merchant not found. Cannot log in.";
                    return authModel;

                }
                if (merchant.IsSubscriped)
                {
                    authModel.isSubscribed = true;
                }
                else
                {
                    authModel.Message = "User is a merchant but not subscribed.";
                    authModel.isSubscribed = false;
                }

            }
            else
            {
                authModel.isSubscribed = true;
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
            // Check if the email is already registered
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthModel
                {
                    Message = "Email is already registered!"
                };
            }

            // Check if the password and password confirmation match
            if (model.Password != model.PasswordConfirm)
            {
                return new AuthModel
                {
                    Message = "Passwords don't match!"
                };
            }

            // Extract username from email
            string[] emailParts = model.Email.Split('@');
            string userName = emailParts[0];

            // Create a new Consumer object
            var consumer = new Consumer
            {
                Email = model.Email,
                UserName = userName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
            };

            // Attempt to create the new consumer
            var result = await _userManager.CreateAsync(consumer, model.Password);
            if (!result.Succeeded)
            {
                // Return error message if creation fails
                return new AuthModel
                {
                    Message = "Failed to create consumer. Please try again."
                };
            }

            // Add the consumer to the "Consumer" role
            await _userManager.AddToRoleAsync(consumer, "Consumer");

            // Create a new shopping cart for the consumer
            var shoppingCartId = await _shoppingCartService.CreateAsync(consumer.Id);
            var shoppingCart = await _shoppingCartService.ReadByIdAsync(shoppingCartId);

            // Add the shopping cart to the consumer's carts
            consumer.ShoppingCarts.Add(shoppingCart);

            // Create a wishlist for the consumer
            var wishList = await _wishListService.CreateWishListAsync(consumer);
            consumer.WishList = wishList;

            // Update the shopping cart with the consumer ID
            shoppingCart.ConsumerId = consumer.Id;
            _context.Update(shoppingCart);

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database update exceptions here
                // Log or handle the exception as needed
                return new AuthModel
                {
                    Message = "An error occurred while saving changes."
                };
            }

            // Create a JWT token for the consumer
            var jwtSecurityToken = await CreateJwtToken(consumer);

            // Return successful authentication response
            // Return authentication details upon successful signup
            return new AuthModel
            {
                Email = consumer.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Consumer" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = userName,
                isSubscribed = true
            };
        }
        public async Task<string> SignupMerchantAsync(SignupMerchantModel model)
        {
            // if the password and the password Confirm didn't match
            if (model.Password != model.PasswordConfirm)
            {
                return "Passwords Don't Match!";
            }
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
                return "Only one ID is needed";
            }
            if (!IsValid)
            {
                return "non Valid!";
            }
            if (NAT)
            {
                // send data to consumer
                var info = NationalIDValidator.ExtractFormattedInfo(model.NationalId);
                //await _mailingService.SendEmailAsync(model.Email, "Data check", info, null);
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
                TRN = (TRN)? model.TaxRegistrationNumber : null,
                BrandName = model.BrandName,
                phoneNumber = model.PhoneNumber
            };
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
        public async Task ResetPasswordAsync(string email, string oldPassword, string newPassword, DateOnly dateOfBirth)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (user.DateOfBirth != dateOfBirth)
            {
                throw new Exception("Date of Birth is not right .. U expect me to believe u forgot you birthday? come on .. Hacker!");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isPasswordCorrect)
            {
                throw new Exception("Incorrect old password.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Password reset failed.");
            }
        }

    }
}
