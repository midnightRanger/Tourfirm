using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;

namespace Tourfirm.Service.Interfaces;

public interface IOrderService
{
    Task<BaseResponse<bool>> MakeOrder(User user, Cart cart);
}