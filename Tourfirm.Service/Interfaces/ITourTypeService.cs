using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface ITourTypeService
{
    Task<BaseResponse<bool>> CreateTourType(TourType tourType);
    Task<BaseResponse<bool>> UpdateTourType(TourType tourType);
    Task<BaseResponse<bool>> DeleteTourType(TourType tourType);
}