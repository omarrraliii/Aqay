using aqay_apis.Models;

namespace aqay_apis.Services
{
    public interface IAdminService
    {
        Task<PaginatedResult<PendingMerchant>> ListAllPendingMerchantsAsync(int pageindex);
        Task<string> AcceptMerchantAsync(int pendingMerchantId);
        Task<string> RejectMerchantAsync(int pendingMerchantId);
        Task<string> ToggleActivityAsync(string id);
        Task<PaginatedResult<Consumer>> ListAllConsumersAsync(int pageIndex);
        Task<PaginatedResult<Merchant>> ListAllMerchantsAsync(int pageIndex);
        Task<PaginatedResult<Brand>> ListAllBrandsAsync(int pageIndex);
        Task<string> GetEmailByUserIDAsync (string id);
    }
}
