using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Controllers;

public class TourController : Controller
{
    private readonly ILogger<TourController> _logger;
    private readonly ApplicationContext _db;
    private readonly ITour _tourRepository;
    private readonly IUser _userRepository;
    private readonly IReview _reviewRepository;

    public TourController(ApplicationContext db, ILogger<TourController> logger, ITour tourRepository, IUser userRepository, IReview reviewRepository)
    {
        _db = db;
        _logger = logger;
        _tourRepository = tourRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
    }


    [HttpGet]
    public async Task<IActionResult> TourInfo(int? id)
    {
        var allTours = _tourRepository.getAll().Include(t=>t.Reviews).ThenInclude(r=>r.User).Include(t => t.TourImages).Include(t => t.Country)
            .Include(t => t.Hotel).ThenInclude(t=>t.HotelProperties).ThenInclude(t=>t.HotelServices).Include(t => t.Route).Include(t => t.TourType);
        Tour tour = allTours.FirstOrDefault(p => p.Id == id);
        TempData["TourId"] = id;
        return View(tour); 
    }

    [HttpPost]
    public async Task<IActionResult> ReviewAdd(Review review)
    {
        review.TourId = (int)TempData["TourId"];
        review.IsAccept = true; 
        review.User = _userRepository.getAll().FirstOrDefault(u => u.Account.Login == User.Identity.Name);
        await _reviewRepository.addReview(review);

        return RedirectToAction("TourInfo", "Tour", new { id = review.TourId });
    }
}