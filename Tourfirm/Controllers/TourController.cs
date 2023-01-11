using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.DAL.ViewModels;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.ViewModels;
using Route = Tourfirm.Domain.Entity.Route;

namespace Tourfirm.Controllers;

public class TourController : Controller
{
    private readonly ILogger<TourController> _logger;
    private readonly ApplicationContext _db;
    private readonly ITour _tourRepository;
    private readonly IUser _userRepository;
    private readonly IReview _reviewRepository;
    private readonly IHotel _hotelRepository;
    private readonly ITourType _tourTypeRepository;
    private readonly ICountry _countryRepository;
    private readonly IRoute _routeRepository;

    public TourController(ApplicationContext db, ILogger<TourController> logger, ITour tourRepository, IUser userRepository, IReview reviewRepository, IHotel hotelRepository, ITourType tourTypeRepository, ICountry countryRepository, IRoute routeRepository)
    {
        _db = db;
        _logger = logger;
        _tourRepository = tourRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _tourTypeRepository = tourTypeRepository;
        _countryRepository = countryRepository;
        _routeRepository = routeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> TourAdd(TourAddViewModel tourAddViewModel)
    {
        tourAddViewModel.AllHotels = new(await _hotelRepository.getHotels(), nameof(Hotel.Id), nameof(Hotel.Name));
        tourAddViewModel.AllCountries = new(await _countryRepository.getCountries(), nameof(Country.Id), nameof(Country.Name));
        tourAddViewModel.AllRoutes = new(await _routeRepository.getRoutes(), nameof(Route.Id), nameof(Route.EndPost));
        tourAddViewModel.AllTourTypes = new(await _tourTypeRepository.getTourTypes(), nameof(TourType.Id), nameof(TourType.Name));
        return View(tourAddViewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> TourInfo(int? id)
    {
        var allTours = _tourRepository.getAll().Include(t=>t.Reviews).ThenInclude(r=>r.User).Include(t => t.TourImages).Include(t => t.Country)
            .Include(t => t.Hotel).ThenInclude(t=>t.HotelProperties).ThenInclude(t=>t.HotelServices).Include(t => t.Route).Include(t => t.TourType);
        Tour tour = allTours.FirstOrDefault(p => p.Id == id);
        
        if (tour != null)
        {
            tour.Reviews = tour.Reviews.Where(r => r.IsAccept).ToList();
            TempData["TourId"] = id;
            ModelState.AddModelError("Wait for moderation", "Wait for validation");
            return View(tour);
        }

        return NotFound(); 
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