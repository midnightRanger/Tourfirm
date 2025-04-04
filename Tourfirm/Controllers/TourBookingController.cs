using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Controllers;

public class TourBookingController : Controller
{
    private readonly ILogger<TourBookingController> _logger;
    private readonly ITourBooking _tourBookingRepository;
    private readonly ITourBookingService _tourBookingService;

    public TourBookingController(ILogger<TourBookingController> logger, ITourBooking tourBookingRepository, ITourBookingService tourBookingService)
    {
        _logger = logger;
        _tourBookingRepository = tourBookingRepository;
        _tourBookingService = tourBookingService;
    }

    [Authorize(Roles = "ADMIN,MODERATOR,MANAGER")]
    public async Task<IActionResult> TourBookingIndex(string? notification)
    {
        if (notification != null)
            ModelState.AddModelError("", notification);

        List<TourBooking> tourBookings = await _tourBookingRepository.getQuery().Include(t => t.Tour).Include(t => t.User).ToListAsync();

        return View(tourBookings);
    }

    public async Task<IActionResult> DeleteTourBooking(int id)
    {
        var response = await _tourBookingService.DeleteTourBooking(id);

        return RedirectToAction("TourBookingIndex", "TourBooking", new { notification = response.Description });
    }

    public async Task<IActionResult> ConfirmTourBooking(int id)
    {
        var response = await _tourBookingService.ConfirmTourBooking(id);

        return RedirectToAction("TourBookingIndex", "TourBooking", new { notification = response.Description });
    }

}