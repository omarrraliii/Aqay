using aqay_apis.Context;
using aqay_apis.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis
{
    public class WishListService : IWishListService
    {
        private readonly ApplicationDbContext _context;

        public WishListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishList> AddProductToWishList(int id, int productVariantId)
        {
            var wishList = await _context.WishLists.Include(w => w.ProductsVariants)
                                                   .FirstOrDefaultAsync(w => w.Id == id);
            if (wishList == null)
            {
                throw new Exception($"Wishlist with ID {id} not found");
            }

            var productVariant = await _context.ProductVariants.FindAsync(productVariantId);
            if (productVariant == null)
            {
                throw new Exception($"ProductVariant with ID {productVariantId} not found");
            }

            wishList.ProductsVariants.Add(productVariant);
            await _context.SaveChangesAsync();
            return wishList;
        }

        public async Task<WishList> CreateWishListAsync(string consumerId)
        {
            var wishList = new WishList
            {
                ConsumerId = consumerId
            };
            _context.WishLists.Add(wishList);
            await _context.SaveChangesAsync();
            return wishList;
        }

        public async Task<WishList> RemoveProductFromWishList(int id, int productVariantId)
        {
            var wishList = await _context.WishLists.Include(w => w.ProductsVariants)
                                                   .FirstOrDefaultAsync(w => w.Id == id);
            if (wishList == null)
            {
                throw new Exception($"Wishlist with ID {id} not found");
            }

            var productVariant = await _context.ProductVariants.FindAsync(productVariantId);
            if (productVariant == null)
            {
                throw new Exception($"ProductVariant with ID {productVariantId} not found");
            }

            if (!wishList.ProductsVariants.Remove(productVariant))
            {
                throw new Exception($"ProductVariant with ID {productVariantId} not in wishlist");
            }

            await _context.SaveChangesAsync();
            return wishList;
        }

        public async Task<List<ProductVariant>> GetProductVariantsForWishList(int id)
        {
            var wishList = await _context.WishLists.Include(w => w.ProductsVariants)
                                                   .FirstOrDefaultAsync(w => w.Id == id);
            if (wishList == null)
            {
                throw new Exception($"Wishlist with ID {id} not found");
            }

            return wishList.ProductsVariants.ToList();
        }

        public async Task<int?> GetWishListIdByConsumerIdAsync(string consumerId)
        {
            var wishList = await _context.WishLists
                                         .FirstOrDefaultAsync(w => w.ConsumerId == consumerId);
            return wishList?.Id;
        }
    }
}
