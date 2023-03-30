using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface ITourBookingService
{
    Task<BaseResponse<bool>> AddServiceToBooking(Tour tour, TourAddViewModel tourAddViewModel);
    Task<BaseResponse<bool>> DeleteServiceFromBooking(Tour tour, TourAddViewModel tourAddViewModel);
    Task<BaseResponse<bool>> CreateTourBooking(Tour tour, TourAddViewModel tourAddViewModel);
}