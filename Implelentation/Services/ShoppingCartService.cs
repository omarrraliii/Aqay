using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;
namespace aqay_apis.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync()
        {
            var shoppingCart = new ShoppingCart()
            {
                TotalPrice = 0,
                ConsumerId = null,
                DeliveryFees = 0,
                ProductVariantIds = new List<int>()
            };
            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _context.SaveChangesAsync();
            return shoppingCart.Id;
        }
        public async Task<bool> SetConsumerIdAsync(int shoppingCartId, string consumerId)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(shoppingCartId);
            if (shoppingCart == null) return false;

            shoppingCart.ConsumerId = consumerId;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart != null)
            {
                _context.ShoppingCarts.Remove(shoppingCart);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ShoppingCart> ReadByIdAsync(int id)
        {
            return await _context.ShoppingCarts.FindAsync(id);
        }
        public async Task<ICollection<ShoppingCart>> ReadAllAsync()
        {
            return await _context.ShoppingCarts.ToListAsync();
        }
        public async Task<bool> AddProductVariantAsync(int shoppingCartId, int productVariantId)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(shoppingCartId);
            if (shoppingCart != null)
            {
                shoppingCart.ProductVariantIds.Add(productVariantId);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveProductVariantAsync(int shoppingCartId, int productVariantId)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(shoppingCartId);
            if (shoppingCart != null)
            {
                var variant = shoppingCart.ProductVariantIds.FirstOrDefault(pvId => pvId == productVariantId);
                if (variant != 0)
                {
                    shoppingCart.ProductVariantIds.Remove(variant);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<IList<ProductVariant>> GetProductVariantsAsync(int shoppingCartId)
        {

            // retrive cart object
            var cart = await _context.ShoppingCarts.FindAsync(shoppingCartId);
            IList<int> productVariantsIds = cart.ProductVariantIds;
        
            if (cart == null) return null;
            IList<ProductVariant> productVariants = new List<ProductVariant>();
            foreach (var id in productVariantsIds)
            {
                var variant = await _context.ProductVariants.FindAsync(id);
                if (variant!= null)
                {
                    productVariants.Add(variant);
                }
                
            }
            return productVariants;
        }
    }
}
