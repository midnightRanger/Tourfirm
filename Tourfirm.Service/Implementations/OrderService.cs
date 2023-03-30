using Microsoft.Extensions.Logging;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Service.Interfaces;
using Exception = System.Exception;

namespace Tourfirm.Service.Implementations;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly ApplicationContext _db;
    private readonly IUser _userRepository;
    private readonly ICart _cartRepository;
    private readonly ICheque _chequeRepository;

    public OrderService(ILogger<OrderService> logger, ApplicationContext db, IUser userRepository, ICart cartRepository, ICheque chequeRepository)
    {
        _logger = logger;
        _db = db;
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _chequeRepository = chequeRepository;
    }

    public async Task<BaseResponse<bool>> MakeOrder(TourBooking tourBooking, Cart cart)
    {
        try
        {

            Cheque cheque = new Cheque()
            {
                Sum = tourBooking.TotalCost,
                TourId = tourBooking.TourId,
                Tour = tourBooking.Tour,
                DateTime = DateTime.Now,
                UserId = tourBooking.UserId
            };
                
            cart.Tours.Remove(tourBooking.Tour);
            _cartRepository.updateCart(cart);

            await _chequeRepository.addCheque(cheque);
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Order was successfully confirmed"
            };
        }

        catch(Exception ex)
        {
            _logger.LogError(ex, $"[Order]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}