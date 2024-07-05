namespace aqay_apis.Services
{
    public interface IPaymentService
    {   
        Task<string> GetAuthTokenAsync();
        Task<int> RegisterOrderAsync(string authToken);
        Task<string> GeneratePaymentKeyAsync(string authToken);

    }
}

 