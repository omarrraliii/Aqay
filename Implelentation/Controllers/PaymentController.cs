using aqay_apis.Models.Paymob;
using Microsoft.AspNetCore.Mvc;
using aqay_apis.Services;
using System.Text.Json;
namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymobService;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService, IConfiguration configuration)
        {
            _paymobService = paymentService;
            _configuration = configuration;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay()
        {
            // Get auth token
            var authToken = await _paymobService.GetAuthTokenAsync();

            // Register order
            var orderId = await _paymobService.RegisterOrderAsync(authToken);

            // L7ad hena shaghaaal, mn awel l payment key wel redirection b2a ehh

            // Generate payment key with hardcoded billing data
            var paymentKeyRequest = new PaymentKeyRequest
            {
                AuthToken = authToken,
                AmountCents = 10000,
                Expiration = 3600,
                OrderId = orderId,
                BillingData = new BillingData
                {
                    Apartment = "803",
                    Email = "claudette09@exa.com",
                    Floor = "42",
                    FirstName = "Clifford",
                    Street = "Ethan Land",
                    Building = "8028",
                    PhoneNumber = "+86(8)9135210487",
                    ShippingMethod = "PKG",
                    PostalCode = "01898",
                    City = "Jaskolskiburgh",
                    Country = "CR",
                    LastName = "Nicolas",
                    State = "Utah"
                },
                Currency = "EGP",
                IntegrationId = _configuration.GetValue<int>("Paymob:IntegrationId"),
                LockOrderWhenPaid = false
            };
            var json = JsonSerializer.Serialize(paymentKeyRequest);
            var paymentKey = await _paymobService.GeneratePaymentKeyAsync(authToken);


            //var paymentKey = await _paymobService.GeneratePaymentKeyAsync(authToken, paymentKeyRequest);
            var iframeId = _configuration.GetValue<string>("Paymob:IframeId");

            // Redirect to Paymob payment page
            var paymentUrl = $"https://accept.paymobsolutions.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";

            //Redirect(paymentUrl)
            return Ok(new { paymentUrl });
            //Ok(paymentKey)
            //Ok(orderId)
        }


    }

}
