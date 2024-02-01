using Aqay_v2.Models;

namespace Aqay_v2.Services
{
    public interface IAuthService
    {
        Task<AuthModel> SignupAsync(SignupModel model);

    }
}
