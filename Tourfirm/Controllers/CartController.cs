using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Controllers;

[Authorize(Roles="USER,ADMIN,MODERATOR")]
public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly ApplicationContext _db;
    private readonly IUser _userRepository;
    private readonly List<User> _userList;
    private readonly ICartService _cartService;

    public CartController(ILogger<CartController> logger, ApplicationContext db, IUser userRepository, ICartService cartService)
    {
        _logger = logger;
        _db = db;
        _userRepository = userRepository;
        _cartService = cartService;
        _userList = userRepository.getAll().Include(u => u.Account).Include(u=>u.Cart).ThenInclude(c=>c.Tours).ThenInclude(c=>c.TourImages).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        User? user = _userList.FirstOrDefault(u => u.Account?.Login == User.Identity.Name);
        return View(user.Cart); 
    }
    
    public async Task<IActionResult> DeleteFromCart(int? id)
    {
        User? user = _userList.FirstOrDefault(u => u.Account?.Login == User.Identity.Name);

        var response = await _cartService.DeleteFromCart(id, user.Cart);
        
        if (response.StatusCode == Domain.Safety.StatusCode.OK) 
            return RedirectToAction("Cart", "Cart");
        
        return RedirectToAction("Cart", "Cart", new {notification = response.Description});

    }
    
    public async Task<IActionResult> AddToCart(int? id)
    {
        User? user = _userList.FirstOrDefault(u => u.Account?.Login == User.Identity.Name);

        if (user != null)
        {
            var response = await _cartService.AddToCart(id, user);
        
            if (response.StatusCode == Domain.Safety.StatusCode.OK)
            {
                return RedirectToAction("Cart", "Cart");
            }
            //TODO normal response transfer
            return RedirectToAction("Main", "Home", new { error = response.Description  });
        }
        return RedirectToAction("Main", "Home", new { error = "User is null!"  });
    }
    
    
}