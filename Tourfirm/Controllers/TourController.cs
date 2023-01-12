using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.DAL.ViewModels;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Safety;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;
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
    private readonly ITourService _tourService;
    private readonly ITourImage _tourImageRepository;
    private readonly IWebHostEnvironment _app;

    public TourController(ApplicationContext db, ILogger<TourController> logger, ITour tourRepository, IUser userRepository, IReview reviewRepository, IHotel hotelRepository, ITourType tourTypeRepository, ICountry countryRepository, IRoute routeRepository, IWebHostEnvironment app, ITourImage tourImageRepository, ITourService tourService)
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
        _app = app;
        _tourImageRepository = tourImageRepository;
        _tourService = tourService;
    }

    [HttpGet]
    public async Task<IActionResult> TourAdd(string? notification)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);

        TourAddViewModel tourAddViewModel = new(); 
        tourAddViewModel.AllHotels = new(await _hotelRepository.getHotels(), nameof(Hotel.Id), nameof(Hotel.Name));
        tourAddViewModel.AllCountries = new(await _countryRepository.getCountries(), nameof(Country.Id), nameof(Country.Name));
        tourAddViewModel.AllRoutes = new(await _routeRepository.getRoutes(), nameof(Route.Id), nameof(Route.EndPost));
        tourAddViewModel.AllTourTypes = new(await _tourTypeRepository.getTourTypes(), nameof(TourType.Id), nameof(TourType.Name));

        return View(tourAddViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> TourAdd(TourAddViewModel tourAddViewModel, Tour tour)
    {
        if (!ModelState.IsValid)
        { 
            tourAddViewModel.AllHotels = new(await _hotelRepository.getHotels(), nameof(Hotel.Id), nameof(Hotel.Name));
            tourAddViewModel.AllCountries = new(await _countryRepository.getCountries(), nameof(Country.Id), nameof(Country.Name));
            tourAddViewModel.AllRoutes = new(await _routeRepository.getRoutes(), nameof(Route.Id), nameof(Route.EndPost));
            tourAddViewModel.AllTourTypes = new(await _tourTypeRepository.getTourTypes(), nameof(TourType.Id), nameof(TourType.Name));

            return View(tourAddViewModel);
        }

        var response = await _tourService.CreateTour(tour, tourAddViewModel);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("Main", "Home", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("TourAdd", "Tour", new { notification = response.Description });
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