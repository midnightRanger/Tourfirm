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
   private readonly ITour _tourRepository;
   private readonly IOrderService _orderService;
   public TourBookingService(ILogger<ITourBookingService> logger, ITourBooking tourBookingRepository, IHotelService hotelService, ITour tourRepository, IOrderService orderService)
   {
      _logger = logger;
      _tourBookingRepository = tourBookingRepository;
      _hotelService = hotelService;
      _tourRepository = tourRepository;
      _orderService = orderService;
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

   public async Task<BaseResponse<bool>> CreateTourBooking(int tourId, TourBookingViewModel tourBookingViewModel)
   {
      try
      {
         TourBooking tourBooking = await _tourBookingRepository.getQuery().Include(t => t.HotelServices)
            .SingleOrDefaultAsync(t => t.TourId == tourId);
         
         Tour tour = await _tourRepository.getAll().Include(t => t.Hotel)
            .SingleOrDefaultAsync(t => t.Id == tourBooking.TourId);
         tourBooking.IsConfirmed = false;
         tourBooking.IsOnModerate = true;

         tourBooking.BookingTime = DateTime.Now;
         tourBooking.ArrivalTime = tourBookingViewModel.ArrivalTime;
         tourBooking.SleepingPlaceValue = tourBookingViewModel.SleepingPlaceValue;

         double totalServiceCost = 0.0;
         foreach (var service in tourBooking.HotelServices)
            totalServiceCost += service.Cost;
         ;

         tourBooking.TotalCost = tourBookingViewModel.SleepingPlaceValue * tour.Hotel.CostForBed + tour.Cost + totalServiceCost;

         _tourBookingRepository.updateTourBooking(tourBooking);
         
         return new BaseResponse<bool>()
         {
            Data = true,
            StatusCode = StatusCode.OK,
            Description = "Booking was successfully registered! Please, wait while our personal will accept your request!"
         };
         
      } catch(Exception ex)
      {
         _logger.LogError(ex, $"[Tour Booking Service]: {ex.Message}");
         return new BaseResponse<bool>()
         {
            Description = ex.Message,
            StatusCode = StatusCode.InternalServerError
         };
      }
      
   }

   public async Task<BaseResponse<bool>> DeleteTourBooking(int tourBookingId)
   {
      try
      {
        _tourBookingRepository.deleteTourBooking(tourBookingId); 
         
         return new BaseResponse<bool>()
         {
            Data = true,
            StatusCode = StatusCode.OK,
            Description = "This booking was successfully deleted!"
         };
         
      } catch(Exception ex)
      {
         _logger.LogError(ex, $"[Tour Booking Deleting]: {ex.Message}");
         return new BaseResponse<bool>()
         {
            Description = ex.Message,
            StatusCode = StatusCode.InternalServerError
         };
      }
      
   }

   public async Task<BaseResponse<bool>> ConfirmTourBooking(int tourBookingId)
   {
      try
      {
         TourBooking tourBooking = await _tourBookingRepository.getQuery().Include(t=>t.Tour).
            Include(t=>t.User)
            .ThenInclude(t=>t.Cart).ThenInclude(t=>t.Tours).SingleOrDefaultAsync(t=>t.Id == tourBookingId);

         if (tourBooking == null) 
            return new BaseResponse<bool>()
            {
               Description = "Tour booking is null!",
               StatusCode = StatusCode.InternalServerError
            };
         
         var response = await _orderService.MakeOrder(tourBooking, tourBooking.User.Cart);

         if (response.StatusCode != StatusCode.OK)
            return new BaseResponse<bool>()
            {
               Description = response.Description,
               StatusCode = StatusCode.InternalServerError
            };

         tourBooking.IsConfirmed = true;
         tourBooking.IsOnModerate = false; 
         _tourBookingRepository.updateTourBooking(tourBooking);
         
         return new BaseResponse<bool>()
         {
            Data = true,
            StatusCode = StatusCode.OK,
            Description = "This booking was successfully confirmed!"
         };
         
      } catch(Exception ex)
      {
         _logger.LogError(ex, $"[Tour Booking Deleting]: {ex.Message}");
         return new BaseResponse<bool>()
         {
            Description = ex.Message,
            StatusCode = StatusCode.InternalServerError
         };
      }
   }
}