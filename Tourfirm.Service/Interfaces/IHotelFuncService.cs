using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.ViewModels;

namespace Tourfirm.Service.Interfaces;

public interface IHotelFuncService
{
    Task<BaseResponse<bool>> CreateHotel(HotelAddViewModel hotelAddViewModel);
    Task<BaseResponse<bool>> UpdateHotel(Hotel hotel);
    Task<BaseResponse<bool>> DeleteHotel(Hotel hotel);
}