using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByConsumerIdAsync(string consumerId);
        Task<IEnumerable<Order>> GetOrdersByMerchantIdAsync(int merchantId);
        Task<Order> GetOrderByIdAsync(int id);
        Task<bool> ChangeOrderStatusAsync(int orderId, ORDERSTATUSES newStatus);
        Task<Order> CreateOrderAsync(string consumerId,int productId, string address);
        Task<bool> AcceptOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByMerchantAndStatusAsync(int brandId, ORDERSTATUSES status);
        Task<IEnumerable<Order>> GetOrderHistoryByConsumerIdAsync(string consumerId, ORDERSTATUSES status);
        Task<double> ApplyPromoCodeAsync(string promoCode, double productPrice);
        Task<PromoCode> CreatePromoCodeAsync(PromoCodeDto promoCodeDto);
        Task<bool> CheckoutAsync(int shoppingCartId, string promoCode, string address);
    
    }
}
