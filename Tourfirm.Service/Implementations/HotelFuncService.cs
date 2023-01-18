using Microsoft.Extensions.Logging;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;

public class HotelFuncService : IHotelFuncService
{
    private readonly IHotelProperties _hotelPropertiesRepository;
    private readonly IHotel _hotelRepository;
    private readonly ILogger<HotelFuncService> _logger; 
    private readonly ApplicationContext _db;

    public HotelFuncService(IHotel hotelRepository, ApplicationContext db, ILogger<HotelFuncService> logger, IHotelProperties hotelPropertiesRepository)
    {
        _hotelRepository = hotelRepository;
        _db = db;
        _logger = logger;
        _hotelPropertiesRepository = hotelPropertiesRepository;
    }

    public async Task<BaseResponse<bool>> CreateHotel(HotelAddViewModel hotelAddViewModel)
    {
        try
        {
            HotelProperties hotelProperties = new HotelProperties()
            {
                BookingTypeId = hotelAddViewModel.BookingTypeId,
                Capacity = hotelAddViewModel.Capacity,
                Classification = hotelAddViewModel.Classification,
                Food = hotelAddViewModel.Food,
                Stars = hotelAddViewModel.Stars,
                HotelServices = null
            };
            hotelProperties.Style ??= "Modern";
            await _hotelPropertiesRepository.addHotelProperties(hotelProperties);
            await _db.SaveChangesAsync();
            await _hotelRepository.addHotel(new Hotel()
            {
               HotelProperties = hotelProperties,
               HotelPropertiesId = hotelProperties.Id,
               Name = hotelAddViewModel.Name
            });
            await _db.SaveChangesAsync();
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Hotel creation procedure was successfully completed"
            };
        }

        catch(Exception ex)
        {
            _logger.LogError(ex, $"[Creating Hotel Procedure]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public Task<BaseResponse<bool>> UpdateHotel(Hotel hotel)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<bool>> UpdateHotel(HotelAddViewModel hotelModel)
    {
        try
        {
            if (hotelModel == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TourNotFound,
                    Description = "Hotel not found"
                };
            }
            
            _hotelPropertiesRepository.updateHotelProperties(hotelProperties);
            _hotelRepository.updateHotel(hotel); 
            
            await _db.SaveChangesAsync(); 
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Hotel was updated"
            };
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Update Hotel]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        } 
    }

    public Task<BaseResponse<bool>> DeleteHotel(Hotel hotel)
    {
        throw new NotImplementedException();
    }
}