using aqay_apis.Models;
using aqay_apis.Dtos;
namespace aqay_apis.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync( int productId);
        Task<Review> GetById(int id);
        Task <Review> ReviewProductAsync(ReviewDto review);
    }
}
