using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;

public class TourBookingService : ITourBookingService
{
   private readonly ILogger<ITourBookingService> _logger;
   private readonly ITourBooking _tourBookingRepository;
   private readonly IHotelService _hotelService;

   public TourBookingService(ILogger<ITourBookingService> logger, ITourBooking tourBookingRepository, IHotelService hotelService)
   {
      _logger = logger;
      _tourBookingRepository = tourBookingRepository;
      _hotelService = hotelService;
   }

   public async Task<BaseResponse<bool>> AddServiceToBooking(TourBookingViewModel tourBookingViewModel, int tourId)
   {
      try
      {
         TourBooking tourBooking = await _tourBookingRepository.getQuery()
            .SingleOrDefaultAsync(t => t.TourId == tourId);

         tourBooking.HotelServices.Add(await _hotelService.getHotelService(tourBookingViewModel.ServiceId));

         _tourBookingRepository.updateTourBooking(tourBooking);
            
         return new BaseResponse<bool>()
         {
            Data = true,
            StatusCode = StatusCode.OK,
            Description = "Service was successfully added!"
         };
      }   catch(Exception ex)
      {
         _logger.LogError(ex, $"[Tour Booking Service]: {ex.Message}");
         return new BaseResponse<bool>()
         {
            Description = ex.Message,
            StatusCode = StatusCode.InternalServerError
         };
      }
   }

   public async Task<BaseResponse<bool>> DeleteServiceFromBooking(int tourId, int hotelServiceId)
   {
      try
      {
         TourBooking tourBooking = await _tourBookingRepository.getQuery().Include(t=>t.HotelServices)
            .SingleOrDefaultAsync(t => t.TourId == tourId);
        
         tourBooking.HotelServices.Remove(await _hotelService.getHotelService(hotelServiceId));
       
         _tourBookingRepository.updateTourBooking(tourBooking);

         return new BaseResponse<bool>()
         {
            Data = true,
            StatusCode = StatusCode.OK,
            Description = "Service was successfully removed from booking!"
         };
      }   catch(Exception ex)
      {
         _logger.LogError(ex, $"[Tour Booking Service]: {ex.Message}");
         return new BaseResponse<bool>()
         {
            Description = ex.Message,
            StatusCode = StatusCode.InternalServerError
         };
      }
   }

   
}