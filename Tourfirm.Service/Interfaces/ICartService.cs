using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;

namespace Tourfirm.Service.Interfaces;

public interface ICartService
{
    Task<BaseResponse<bool>> AddToCart(int? tourId, User user);
    Task<BaseResponse<bool>> DeleteFromCart(int? tourId, Cart cart);
    Task<BaseResponse<bool>> ClearCart(User user);
}