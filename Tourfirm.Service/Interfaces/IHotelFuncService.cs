using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface IHotelFuncService
{
    Task<BaseResponse<bool>> CreateHotel(HotelAddViewModel hotelAddViewModel);
    Task<BaseResponse<bool>> UpdateHotel(HotelAddViewModel hotelModel);
    Task<BaseResponse<bool>> DeleteHotel(Hotel hotel);
    Task<BaseResponse<bool>> CreateService(HotelServiceAddViewModel hotelModel); 
}