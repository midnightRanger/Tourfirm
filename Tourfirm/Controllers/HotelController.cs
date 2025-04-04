using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;
[Authorize(Roles = "ADMIN,MANAGER")]
public class HotelController : Controller
{
    private readonly ILogger<HotelController> _logger;
    private readonly ApplicationContext _db;
    private readonly IHotel _hotelRepository;
    private readonly IBookingType _bookingType;
    private readonly IHotelFuncService _hotelService;
    private readonly IHotelService _hotelServ;

    public HotelController(ILogger<HotelController> logger, ApplicationContext db,
        IHotel hotelRepository, IBookingType bookingType,
        IHotelFuncService hotelService, IHotelService hotelServ)
    {
        _logger = logger;
        _db = db;
        _hotelRepository = hotelRepository;
        _bookingType = bookingType;
        _hotelService = hotelService;
        _hotelServ = hotelServ;
    }

    [HttpGet]
    public async Task<IActionResult> HotelAdd(string? notification)
    {
        if (notification != null)
            ModelState.AddModelError("", notification);

        HotelAddViewModel hotelAddViewModel = new();
        hotelAddViewModel.AllBookings = new(await _bookingType.getBookingTypes(), nameof(BookingType.Id), nameof(BookingType.Name));
        return View(hotelAddViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> HotelAdd(HotelAddViewModel hotelAddViewModel)
    {
        if (!ModelState.IsValid)
        {
            hotelAddViewModel.AllBookings = new(await _bookingType.getBookingTypes(), nameof(BookingType.Id), nameof(BookingType.Name));
            return View(hotelAddViewModel);
        }

        var response = await _hotelService.CreateHotel(hotelAddViewModel);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("HotelIndex", "Hotel", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("HotelAdd", "Hotel", new { notification = response.Description });
    }

    [HttpGet]
    public async Task<IActionResult> HotelUpdate(string? notification, int id)
    {
        Hotel hotel = _hotelRepository.getAll().Include(h => h.HotelProperties).ThenInclude(h => h.HotelServices).FirstOrDefault(h => h.Id == id);

        if (notification != null)
            ModelState.AddModelError("", notification);

        HotelAddViewModel hotelModel = new()
        {
            Name = hotel.Name,
            AllBookings = new(await _bookingType.getBookingTypes(), nameof(BookingType.Id), nameof(BookingType.Name)),
            Capacity = hotel.HotelProperties.Capacity,
            Classification = hotel.HotelProperties.Classification,
            Food = hotel.HotelProperties.Food,
            Stars = hotel.HotelProperties.Stars,
            Style = hotel.HotelProperties.Style,
            HotelPropertiesId = hotel.HotelPropertiesId
        };

        return View(hotelModel);
    }

    [HttpPost]
    public async Task<IActionResult> HotelUpdate(HotelAddViewModel hotelModel)
    {
        if (!ModelState.IsValid)
        {

            return View(hotelModel);
        }

        Hotel hotel = _hotelRepository.getAll().Include(h => h.HotelProperties).FirstOrDefault(h => h.Id == hotelModel.Id);

        hotelModel.HotelPropertiesId = hotel.HotelPropertiesId;
        var response = await _hotelService.UpdateHotel(hotelModel);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("HotelIndex", "Hotel", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("HotelUpdate", "Hotel", new { notification = response.Description });

    }


    public async Task<IActionResult> HotelIndex(string? notification, Hotel.SortState sortHotel = Hotel.SortState.IdAsc)
    {
        if (notification != null)
            ModelState.AddModelError("", notification);

        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdHotel"] = sortHotel == Hotel.SortState.IdAsc ? Hotel.SortState.IdDesc : Hotel.SortState.IdAsc;
        IQueryable<Hotel> hotels = _hotelRepository.getAll().Include(h => h.HotelProperties).
            ThenInclude(h => h.HotelServices).
            Include(h => h.HotelProperties).ThenInclude(h => h.BookingType);
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

    [HttpGet]
    public async Task<IActionResult> HotelDeleteConfirm(int id)
    {
        return View(await _hotelRepository.getHotel(id));
    }


    public async Task<IActionResult> HotelDelete(int id)
    {
        var response = await _hotelService.DeleteHotel(await _hotelRepository.getHotel(id));

        return RedirectToAction("HotelIndex", "Hotel", new { notification = response.Description });

    }


    [HttpGet]
    public async Task<IActionResult> HotelService(int id)
    {
        Hotel hotel = await _hotelRepository.getAll().Include(h => h.HotelProperties).ThenInclude(h => h.HotelServices).FirstOrDefaultAsync(h => h.Id == id);
        HotelServiceAddViewModel hotelService = new HotelServiceAddViewModel()
        {
            HotelServices = await _hotelServ.getAll().Where(h => h.HotelPropertiesId == hotel.HotelPropertiesId)
                .ToListAsync(),
            HotelId = id
        };

        return View(hotelService);
    }

    [HttpPost]
    public async Task<IActionResult> HotelServiceUpdate(HotelService hotelModel, int propertiesId)
    {
        Hotel hotel = await _hotelRepository.getAll().Where(h => h.HotelPropertiesId == propertiesId).FirstOrDefaultAsync() ?? throw new InvalidOperationException();
        if (!ModelState.IsValid)
        {
            return RedirectToAction("HotelServiceUpdate", "Hotel", new { id = hotel.HotelPropertiesId });
        }

        hotelModel.HotelPropertiesId = propertiesId;
        var response = await _hotelService.UpdateService(hotelModel);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("HotelIndex", "Hotel", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("HotelServiceUpdate", "Hotel", new { notification = response.Description });
    }


    [HttpGet]
    public async Task<IActionResult> HotelServiceUpdate(string? notification, int id)
    {
        if (notification != null)
            ModelState.AddModelError("", notification);

        HotelService hotelService = await _hotelServ.getHotelService(id);

        return View(hotelService);
    }

    [HttpPost]
    public async Task<IActionResult> HotelService(HotelServiceAddViewModel hotelModel, int? id, string? notification)
    {
        if (notification != null)
            ModelState.AddModelError("", notification);
        Hotel hotel = await _hotelRepository.getAll().Include(h => h.HotelProperties).ThenInclude(h => h.HotelServices).FirstOrDefaultAsync(h => h.Id == id);

        if (!ModelState.IsValid)
        {
            HotelServiceAddViewModel hotelService = new HotelServiceAddViewModel()
            {
                HotelServices = await _hotelServ.getAll().Where(h => h.HotelPropertiesId == hotel.HotelPropertiesId)
                    .ToListAsync(),
                HotelId = hotelModel.HotelId
            };
            return View(hotelModel);
        }


        hotelModel.HotelId = hotel.Id;
        hotelModel.HotelPropertiesId = hotel.HotelPropertiesId;

        var response = await _hotelService.CreateService(hotelModel);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("HotelService", "Hotel", new { id = hotelModel.HotelId });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("HotelService", "Hotel", new { id = hotelModel.HotelId, notification = response.Description });
    }

    public async Task<IActionResult> HotelServiceRemove(int id, int hotelId)
    {
        _hotelServ.deleteHotelService(id);
        return RedirectToAction("HotelService", "Hotel", new { id = hotelId });
    }
}