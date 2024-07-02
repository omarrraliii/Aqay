using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public interface IOrderService
    {
        Task<PaginatedResult<Order>> GetOrdersByConsumerIdAsync(string consumerId, int pageNumber);
        Task<PaginatedResult<Order>> GetOrdersByMerchantIdAsync(int merchantId, int pageNumber);
        Task<Order> GetOrderByIdAsync(int id);
        Task<bool> ChangeOrderStatusAsync(int orderId, ORDERSTATUSES newStatus);
        Task<Order> CreateOrderAsync(string consumerId,int productId);
        Task<bool> AcceptOrderAsync(int orderId);
        Task<PaginatedResult<Order>> GetOrdersByMerchantAndStatusAsync(int brandId, ORDERSTATUSES status, int pageNumber);
        Task<PaginatedResult<Order>> GetOrderHistoryByConsumerIdAsync(string consumerId, ORDERSTATUSES status, int pageNumber);
        Task<double> ApplyPromoCodeAsync(string promoCode, double productPrice);
        Task<PromoCode> CreatePromoCodeAsync(PromoCodeDto promoCodeDto);
        Task<bool> CheckoutAsync(int shoppingCartId, string promoCode);

    }
}
