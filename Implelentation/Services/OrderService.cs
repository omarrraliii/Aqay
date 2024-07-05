
using aqay_apis.Context;
using aqay_apis.Models;
using Microsoft.EntityFrameworkCore;
namespace aqay_apis.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductVariantService _productVariantService;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
   
        public OrderService(ApplicationDbContext context, IProductVariantService productVariantService, IProductService productService, IShoppingCartService shoppingCartService)
        {
            _context = context;
            _productVariantService = productVariantService;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }
        public async Task<IEnumerable<Order>> GetOrdersByConsumerIdAsync(string consumerId)
        {
           return await _context.Orders
                .Where(o => o.ConsumerId == consumerId)
                .OrderByDescending(o => o.LastEdit)
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetOrdersByMerchantIdAsync(int merchantId)
        {
            return await _context.Orders
                .Where(o => o.BrandId == merchantId)
                .OrderByDescending(o => o.LastEdit)
                .ToListAsync();

        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            return order;
        }
        public async Task<bool> ChangeOrderStatusAsync(int orderId, ORDERSTATUSES newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.ORDERSTATUSES = newStatus;
            order.LastEdit = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Order> CreateOrderAsync(string consumerId, int productVariantId, string address)
        {
            var productVariant = await _context.ProductVariants.FindAsync(productVariantId);
            if (productVariant == null)
            {
                throw new Exception("Product Variant not found");
            }

            var product = await _context.Products.FindAsync(productVariant.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var consumer = await _context.Users.FindAsync(consumerId);
            if (consumer == null)
            {
                throw new Exception("Consumer not found");
            }
            var brand = await _context.Brands.FindAsync(product.BrandId);
            var brandName = (brand == null) ? " " : brand.Name;
            var consumerName = (consumer.UserName == null) ? " " : consumer.UserName;
            var newOrder = new Order
            {
                ConsumerId = consumerId,
                TotalPrice = product.Price,
                ORDERSTATUSES = ORDERSTATUSES.PENDING,
                CreatedOn = DateTime.Now,
                LastEdit = DateTime.Now,
                BrandId = product.BrandId,
                BrandName = brandName,
                ProductVariantId = productVariantId,
                Address = address,
                ProductName = product.Name,
                ConsumerName = consumerName,
                ConsumerGender = consumer.Gender
            };
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return newOrder;
        }

        public async Task<bool> AcceptOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.IsAccepted = true;
            order.ORDERSTATUSES = ORDERSTATUSES.PROCESSING;
            order.LastEdit = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Order>> GetOrdersByMerchantAndStatusAsync(int brandId, ORDERSTATUSES status)
        {
            return await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == status)
                .OrderByDescending(o => o.LastEdit)
                .ToListAsync();

            
        }
        public async Task<IEnumerable<Order>> GetOrderHistoryByConsumerIdAsync(string consumerId, ORDERSTATUSES status)
        {
           return await _context.Orders
                .Where(o => o.ConsumerId == consumerId && o.ORDERSTATUSES == status)
                .OrderByDescending(o => o.LastEdit)
                .ToListAsync();
        }
        public async Task<PromoCode> CreatePromoCodeAsync(PromoCodeDto promoCodeDto)
        {
            var promoCode = new PromoCode
            {
                Code = promoCodeDto.Code,
                CreatedOn = DateTime.Now,
                ExpDate = promoCodeDto.ExpDate,
                Percentage = promoCodeDto.Percentage,
                Amount = promoCodeDto.Amount
            };

            _context.PromoCodes.Add(promoCode);
            await _context.SaveChangesAsync();

            return promoCode;
        }
        public async Task<double> ApplyPromoCodeAsync(string promoCodeStr, double productPrice)
        {
            var promoCode = await _context.PromoCodes.FirstOrDefaultAsync(pc => pc.Code == promoCodeStr);
            if (promoCode == null)
            {
                throw new Exception("Promo code not found.");
            }

            if (promoCode.ExpDate < DateTime.Now)
            {
                throw new Exception("Promo code has expired.");
            }

            double newPrice = 0;
            if (promoCode.Percentage != 0)
            {
                newPrice = productPrice - (productPrice * (promoCode.Percentage / 100));
            }
            else if (promoCode.Amount != 0)
            {
                newPrice = productPrice - promoCode.Amount;
            }
            else
            {
                throw new Exception("Invalid promo code values.");
            }
            if (newPrice < 0)
            {
                throw new Exception("Price after applying promo code cannot be less than zero.");
            }
            return newPrice;
        }
        public async Task<bool> CheckoutAsync(int shoppingCartId, string promoCode, string address)
        {
            // Get shopping cart by id
            var shoppingCart = await _context.ShoppingCarts.FindAsync(shoppingCartId);
            if (shoppingCart == null)
            {
                throw new Exception("Shopping cart not found.");
            }
            // Get promo code by code
            var promoCodeEntity = await _context.PromoCodes.FirstOrDefaultAsync(pc => pc.Code == promoCode);
            if (promoCodeEntity != null && promoCodeEntity.ExpDate < DateTime.Now)
            {
                throw new Exception("Promo code has expired.");
            }
            // Get consumer id from the shopping cart
            var consumerId = shoppingCart.ConsumerId;

            IList<int> productVariantsIds = shoppingCart.ProductVariantIds;

            // Loop over each product variant inside the shopping cart and create an order with it
            foreach (var productVariantId in productVariantsIds)
            {
                var order = await CreateOrderAsync(consumerId, productVariantId,address);

                if (promoCodeEntity != null)
                {
                    var newPrice = await ApplyPromoCodeAsync(promoCode, order.TotalPrice);
                    order.TotalPrice = newPrice;
                }
                _context.Orders.Update(order);
            }

            // Remove the shopping cart
            await _shoppingCartService.DeleteAsync(shoppingCartId);

            // Create another shopping cart and link it with the consumer of the old one
            var newShoppingCart = new ShoppingCart
            {
                ConsumerId = consumerId,
                TotalPrice = 0,
                DeliveryFees = 0 // to be editied after address
            };
            _context.ShoppingCarts.Add(newShoppingCart);

            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
