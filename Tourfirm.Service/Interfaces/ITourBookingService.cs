using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface ITourBookingService
{
    Task<BaseResponse<bool>> AddServiceToBooking(TourBookingViewModel tourBookingViewModel, int tourId);
    Task<BaseResponse<bool>> DeleteServiceFromBooking(int tourId, int hotelServiceId);
    Task<BaseResponse<bool>> CreateTourBooking(int tourId, TourBookingViewModel tourBookingViewModel);
    Task<BaseResponse<bool>> DeleteTourBooking(int tourBookingId);
    Task<BaseResponse<bool>> ConfirmTourBooking(int tourBookingId); 
}