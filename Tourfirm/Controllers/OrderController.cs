using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Controllers;

public class OrderController : Controller
{
    private readonly ICart _cartRepository;
    private readonly ICheque _chequeRepository;
    private readonly ApplicationContext _db;
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;
    private readonly IUser _userRepository;

    public OrderController(ILogger<OrderController> logger, ApplicationContext db, IUser userRepository,
        ICart cartRepository, ICheque chequeRepository, IOrderService orderService)
    {
        _logger = logger;
        _db = db;
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _chequeRepository = chequeRepository;
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> OrderConfirm(string? notification)
    {
        var user = _userRepository.getAll().Include(w => w.Account).Include(w => w.Cart)
            .FirstOrDefault(w => w.Account.Login == User.Identity.Name);
        var cart = _cartRepository.getAll().Include(c => c.Tours).FirstOrDefault(w => w.Id == user.Cart.Id);

        if (cart.Tours.Count == 0) return RedirectToAction("Cart", "Cart", new { notification = "The cart is empty" });
    

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> MakeOrder()
    {
        if (!ModelState.IsValid) return RedirectToAction("OrderConfirm", "Order");
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        var user = _userRepository.getAll().Include(w => w.Account).Include(w => w.Cart)
            .FirstOrDefault(w => w.Account.Login == User.Identity.Name);
        var cart = _cartRepository.getAll().Include(c => c.Tours).FirstOrDefault(w => w.Id == user.Cart.Id);

        // var response = await _orderService.MakeOrder(user, cart);
        //
        // if (response.StatusCode == Domain.Safety.StatusCode.OK)
        // {
        //         return RedirectToAction("Orders", "Order", new { notification = response.Description });
        // }
        // ModelState.AddModelError("", response.Description);
        return RedirectToAction("OrderConfirm", "Order", new { notification = "Delete later" });
    }

    [HttpGet]
    public async Task<IActionResult> Orders(string? notification)
    {
        var user = _userRepository.getAll().Include(w => w.Account).Include(w => w.Cart)
            .FirstOrDefault(w => w.Account.Login == User.Identity.Name);
        var cheque = await _chequeRepository.getAll().Include(c => c.Tour).Where(w => w.UserId == user.Id)
            .ToListAsync();

        if (notification != null)
            ModelState.AddModelError("", notification);

        return View(cheque);
    }
}