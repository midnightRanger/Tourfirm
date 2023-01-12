using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface ITourService
{
    Task<BaseResponse<bool>> CreateTour(Tour tour, TourAddViewModel tourAddViewModel);
    // Task<BaseResponse<bool>> UpdateTour(Tour tour);
    Task<BaseResponse<bool>> DeleteTour(Tour tour);
}