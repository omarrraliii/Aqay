using aqay_apis.Helpers;
using aqay_apis.Models;
using aqay_apis.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailingService _mailingService;
        private readonly UserManager<User> _userManager;
        private readonly GlobalVariables _globalVariables;
        private readonly IBrandService _brandService;
        public AdminService(ApplicationDbContext context,GlobalVariables globalVariables, UserManager<User> userManager, IMailingService mailingService, IBrandService brandService)
        {
            _context = context;
            _userManager = userManager;
            _mailingService = mailingService;
            _globalVariables = globalVariables;
            _brandService = brandService;
        }
        public async Task<PaginatedResult<PendingMerchant>> ListAllPendingMerchantsAsync(int pageindex)
        {
            var pendingMercants = await _context.Set<PendingMerchant>().Skip((pageindex - 1) * _globalVariables.PageSize)
                        .Take(_globalVariables.PageSize)
                        .ToListAsync();
            var pendingMercantsCount = pendingMercants.Count;
            var PaginatedResult = new PaginatedResult<PendingMerchant>
            {
                Items = pendingMercants,
                TotalCount = pendingMercantsCount,
                HasMoreItems = (pageindex * _globalVariables.PageSize) < pendingMercantsCount
            };
            return PaginatedResult;
        }
        public async Task<string> AcceptMerchantAsync(int pendingMerchantId)
        {
            var pendingMerchant = await _context.Set<PendingMerchant>().FindAsync(pendingMerchantId);
            if (pendingMerchant == null)
            {
                return "Pending merchant not found";
            }

            // Check if there is already a user with the same email
            if (await _userManager.FindByEmailAsync(pendingMerchant.Email) != null)
            {
                return "Email is already registered!";
            }

            // Extract username from email
            string[] emailParts = pendingMerchant.Email.Split('@');
            string userName = emailParts[0];

            // Create a new Merchant object
            var newMerchant = new Merchant
            {
                Email = pendingMerchant.Email,
                UserName = userName,
                IsVerified = true,
                IsOwner = true,
                IsSubscriped = false,
                BrandName = pendingMerchant.BrandName,
                PhoneNumber = pendingMerchant.phoneNumber
            };

            // Attempt to create the new user
            var result = await _userManager.CreateAsync(newMerchant, pendingMerchant.Password);
            if (!result.Succeeded)
            {
                return "Failed to create merchant. Please try again.";
            }

            // Add the user to the "Owner" role
            await _userManager.AddToRoleAsync(newMerchant, "Owner");

            // Create a brand and link it with the accepted merchant
            var brand = await _brandService.CreateBrandAsync(newMerchant.Id);

            // Update the new merchant with the created brand
            newMerchant.Brand = brand;
            await _userManager.UpdateAsync(newMerchant);

            // Remove the pending merchant from the context
            _context.Set<PendingMerchant>().Remove(pendingMerchant);

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database update exceptions here
                // Log or handle the exception as needed
                return "An error occurred while saving changes.";
            }

            return "Merchant accepted. Proceed to the subscription page.";
        }

        public async Task<string> RejectMerchantAsync(int pendingMerchantId)
        {
            var pendingMerchant = await _context.Set<PendingMerchant>().FindAsync(pendingMerchantId);
            if (pendingMerchant == null)
            {
                return "Pending merchant not found";
            }

            _context.Set<PendingMerchant>().Remove(pendingMerchant);
            await _context.SaveChangesAsync();
            //await _mailingService.SendEmailAsync(pendingMerchant.Email, "Merchant Rejected", "Sorry, You have been rejected as a merchant as AQAY\nCheck Tech support for rejection reasons",null);
            return "Merchant rejected";
        }
        public async Task<string> ToggleActivityAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return "User not found";

            user.IsActive = !user.IsActive;

            await _userManager.UpdateAsync(user);

            return $"Activity toggled for user {user.Email} to {user.IsActive}";
        }
        public async Task<PaginatedResult<Consumer>> ListAllConsumersAsync(int pageIndex)
        {
            var consumers = await _context.Consumers
                .Skip((pageIndex - 1) * _globalVariables.PageSize)
                .Take(_globalVariables.PageSize)
                .ToListAsync();

            var totalCount = await _context.Consumers.CountAsync();

            return new PaginatedResult<Consumer>
            {
                Items = consumers,
                TotalCount = totalCount,
                HasMoreItems = (pageIndex * _globalVariables.PageSize) < totalCount
            };
        }
        public async Task<PaginatedResult<Merchant>> ListAllMerchantsAsync(int pageIndex)
        {
            var merchants = await _context.Merchants
                .Skip((pageIndex - 1) * _globalVariables.PageSize)
                .Take(_globalVariables.PageSize)
                .ToListAsync();
            var totalCount = await _context.Merchants.CountAsync();
            return new PaginatedResult<Merchant>
            {
                Items = merchants,
                TotalCount = totalCount,
                HasMoreItems = (pageIndex * _globalVariables.PageSize) < totalCount
            };
        }
        public async Task<PaginatedResult<Brand>> ListAllBrandsAsync(int pageIndex)
        {
            var brands = await _context.Brands
                .Skip((pageIndex - 1) * _globalVariables.PageSize)
                .Take(_globalVariables.PageSize)
                .ToListAsync();

            var totalCount = await _context.Brands.CountAsync();

            return new PaginatedResult<Brand>
            {
                Items = brands,
                TotalCount = totalCount,
                HasMoreItems = (pageIndex * _globalVariables.PageSize) < totalCount
            };
        }

        public async Task<string> GetEmailByUserIDAsync(string id)
        {
            var user=await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user.Email;
        }
    }
}
