using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;
//Сервис для взаимодействия с корзинами
public class CartService : ICartService
{
    private readonly ICart _cartRepository;
    private readonly ITour _tourRepository;
    private readonly IUser _userRepository;
    private readonly ILogger<CartService> _logger;

    public CartService(ICart cartRepository, ILogger<CartService> logger, ITour tourRepository, IUser userRepository)
    {
        _cartRepository = cartRepository;
        _tourRepository = tourRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> DeleteFromCart(int? tourId, Cart cart)
    {
        try
        { 
            Tour tour = cart.Tours.FirstOrDefault(e => e.Id == tourId);

            if (tour == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.KeyNotFound,
                    Description = "There is no tour to delete"
                };
                
            }

            cart.Tours.Remove(tour);  
            _cartRepository.updateCart(cart);
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Item was deleted from the cart"
            };
        }   
        
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[DeleteFromCart]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<bool>> ClearCart(User user)
    {

        if (user.Cart == null || user.Cart.Tours == null)
        {
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.CartNotFound,
                Description = "Something goes wrong"
            };
        }
        foreach (var item in user.Cart.Tours.ToList())
        {
            item.Carts.Remove(user.Cart);
             _tourRepository.updateTour(item);
        }

        return new BaseResponse<bool>()
        {
            Data = true,
            StatusCode = StatusCode.OK,
            Description = "Cart was sucessfully cleared"
        };
    }

    public async Task<BaseResponse<bool>> AddToCart(int? tourId, User user)
    {
        try
        {
            Tour? tour = await _tourRepository.getAll().FirstOrDefaultAsync(k => k.Id == tourId);

            if (tour == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.KeyNotFound,
                    Description = "Sorry, but we dont have this tour"
                };
            }

            if (user.Cart == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.CartNotFound,
                    Description = "You dont have a cart entity. WTF?"
                };
            }
            
            
            user.Cart.Tours.Add(tour);
            _userRepository.updateUser(user);
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Item was added to the cart"
            };
            
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[AddToCart]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}