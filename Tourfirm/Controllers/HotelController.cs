using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;

public class HotelController : Controller
{
    private readonly ILogger<HotelController> _logger;
    private readonly ApplicationContext _db;
    private readonly IHotel _hotelRepository;
    private readonly IBookingType _bookingType;

    public HotelController(ILogger<HotelController> logger, ApplicationContext db, IHotel hotelRepository, IBookingType bookingType)
    {
        _logger = logger;
        _db = db;
        _hotelRepository = hotelRepository;
        _bookingType = bookingType;
    }
    
    [HttpGet]
    public async Task<IActionResult> HotelAdd(string? notification)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);

        HotelAddViewModel hotelAddViewModel = new(); 
        hotelAddViewModel.AllBookings = new(await _bookingType.getBookingTypes(), nameof(BookingType.Id), nameof(BookingType.Name));
        return View(hotelAddViewModel);
    }
    

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