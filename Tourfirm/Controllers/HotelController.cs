using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;

public class HotelController : Controller
{
    private readonly ILogger<HotelController> _logger;
    private readonly ApplicationContext _db;
    private readonly IHotel _hotelRepository;

    public HotelController(ILogger<HotelController> logger, ApplicationContext db, IHotel hotelRepository)
    {
        _logger = logger;
        _db = db;
        _hotelRepository = hotelRepository;
    }

    [HttpGet]
    public IActionResult HotelAdd(string? notification) => View(); 
    

    public async Task<IActionResult> HotelIndex(string? notification, Hotel.SortState sortHotel = Hotel.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdHotel"] = sortHotel == Hotel.SortState.IdAsc ? Hotel.SortState.IdDesc : Hotel.SortState.IdAsc;
        IQueryable<Hotel> hotels = _hotelRepository.getAll().Include(h=>h.HotelProperties).
            ThenInclude(h=>h.HotelServices).
            Include(h=>h.HotelProperties).ThenInclude(h=>h.BookingType);
        
        switch (sortHotel)
        {
            case Hotel.SortState.IdAsc:
            {
                hotels = hotels.OrderBy(p => p.Id);
                break;
            }

            case Hotel.SortState.IdDesc:
            {
                hotels = hotels.OrderByDescending(p => p.Id);
                break;
            }
        }

        return View(await hotels.AsNoTracking().ToListAsync());
    }
}