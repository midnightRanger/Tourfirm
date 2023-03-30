using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Controllers;

public class TourBookingController: Controller
{
    private readonly ILogger<TourBookingController> _logger;
    private readonly ITourBooking _tourBookingRepository;

    public TourBookingController(ILogger<TourBookingController> logger, ITourBooking tourBookingRepository)
    {
        _logger = logger;
        _tourBookingRepository = tourBookingRepository;
    }

    [Authorize(Roles = "ADMIN,MODERATOR,MANAGER")]
    public async Task<IActionResult> TourBookingIndex()
    {
        List<TourBooking> tourBookings = await _tourBookingRepository.getQuery().Include(t => t.Tour).Include(t => t.User).ToListAsync();

        return View(tourBookings);
    }
    
}