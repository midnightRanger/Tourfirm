using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface IRouteService
{
    Task<BaseResponse<bool>> CreateRoute(Route route);
    Task<BaseResponse<bool>> UpdateRoute(Route route);
    Task<BaseResponse<bool>> DeleteRoute(Route route);
}