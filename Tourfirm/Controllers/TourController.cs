using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Authorization;
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
    private readonly ITour? _tourRepository;
    private readonly IUser? _userRepository;
    private readonly IReview? _reviewRepository;
    private readonly IHotel? _hotelRepository;
    private readonly ITourType? _tourTypeRepository;
    private readonly ICountry? _countryRepository;
    private readonly IRoute? _routeRepository;
    private readonly ITourService? _tourService;
    private readonly ITourImage? _tourImageRepository;
    private readonly IHotelService _hotelService;
    private readonly ITourBooking _tourBookingRepository;
  

    public TourController(ILogger<TourController> logger, ITour tourRepository,
        IUser userRepository, IReview reviewRepository, IHotel hotelRepository, 
        ITourType tourTypeRepository, ICountry countryRepository, IRoute routeRepository,
        ITourImage tourImageRepository, ITourService tourService, IHotelService hotelService, ITourBooking tourBookingRepository)
    {
        _logger = logger;
        _tourRepository = tourRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _tourTypeRepository = tourTypeRepository;
        _countryRepository = countryRepository;
        _routeRepository = routeRepository;
        _tourImageRepository = tourImageRepository;
        _tourService = tourService;
        _hotelService = hotelService;
        _tourBookingRepository = tourBookingRepository;
    }

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
    public async Task<IActionResult> ImageRemove(int id, int tourId)
    {
        _tourImageRepository.deleteTourImage(id);
        return RedirectToAction("TourUpdate", "Tour", new {id = tourId });
    }
    
    [HttpGet]
    public async Task<IActionResult> TourIndex(string? notification, Tour.SortState sortTour = Tour.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdSort"] = sortTour == Tour.SortState.IdAsc ? Tour.SortState.IdDesc : Tour.SortState.IdAsc;
        IQueryable<Tour> tours = _tourRepository.getAll();
        
        var allTours = _tourRepository.getAll().Include(t=>t.Reviews).ThenInclude(r=>r.User).Include(t => t.TourImages).Include(t => t.Country)
            .Include(t => t.Hotel).ThenInclude(t=>t.HotelProperties).ThenInclude(t=>t.HotelServices).Include(t => t.Route).Include(t => t.TourType);

        switch (sortTour)
        {
            case Tour.SortState.IdAsc:
            {
                tours = allTours.OrderBy(p => p.Id);
                break;
            }

            case Tour.SortState.IdDesc:
            {
                tours = allTours.OrderByDescending(p => p.Id);
                break;
            }
        }

        return View("TourIndex", tours.AsNoTracking().ToList());

    }

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
    [HttpGet]
    public async Task<IActionResult> TourDeleteConfirm(int id)
    {
        return View(await _tourRepository.getTour(id));
    }

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
    public async Task<IActionResult> TourDelete(int? id)
    {
        var response = await _tourService.DeleteTour(await _tourRepository.getTour(id));

        return RedirectToAction("TourIndex", "Tour", new { notification = response.Description});
        
    }

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
    [HttpGet]
    public async Task<IActionResult> TourUpdate(string? notification, int? id)
    {
        IQueryable<Tour> tours = _tourRepository.getAll().Include(t=>t.TourImages);
        var tour = await tours.FirstOrDefaultAsync(t=>t.Id == id);
        
        if(notification != null)
            ModelState.AddModelError("", notification);

        TourUpdateViewModel tourUpdateViewModel = new(); 
        tourUpdateViewModel.AllHotels = new(await _hotelRepository.getHotels(), nameof(Hotel.Id), nameof(Hotel.Name));
        tourUpdateViewModel.AllCountries = new(await _countryRepository.getCountries(), nameof(Country.Id), nameof(Country.Name));
        tourUpdateViewModel.AllRoutes = new(await _routeRepository.getRoutes(), nameof(Route.Id), nameof(Route.EndPost));
        tourUpdateViewModel.AllTourTypes = new(await _tourTypeRepository.getTourTypes(), nameof(TourType.Id), nameof(TourType.Name));

        tourUpdateViewModel.Cost = tour.Cost;
        tourUpdateViewModel.Description = tour.Description;
        tourUpdateViewModel.Name = tour.Name;
        tourUpdateViewModel.Id = tour.Id; 

        foreach (var image in tour.TourImages)
        {
            tourUpdateViewModel.Images.Add(image);
        }
        
        
        return View(tourUpdateViewModel);
    }

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
    [HttpPost]
    public async Task<IActionResult> TourUpdate(TourUpdateViewModel tourUpdateViewModel, Tour tour)
    {
        if (!ModelState.IsValid)
        { 
            tourUpdateViewModel.AllHotels = new(await _hotelRepository.getHotels(), nameof(Hotel.Id), nameof(Hotel.Name));
            tourUpdateViewModel.AllCountries = new(await _countryRepository.getCountries(), nameof(Country.Id), nameof(Country.Name));
            tourUpdateViewModel.AllRoutes = new(await _routeRepository.getRoutes(), nameof(Route.Id), nameof(Route.EndPost));
            tourUpdateViewModel.AllTourTypes = new(await _tourTypeRepository.getTourTypes(), nameof(TourType.Id), nameof(TourType.Name));
            
            IQueryable<Tour> tours = _tourRepository.getAll().Include(t=>t.TourImages);
            var tourModel = await tours.FirstOrDefaultAsync(t=>t.Id == tourUpdateViewModel.Id);
            tourUpdateViewModel.Images = tourModel.TourImages; 
            
            return View(tourUpdateViewModel);
        }

        var response = await _tourService.UpdateTour(tour, tourUpdateViewModel);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("Main", "Home", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("TourUpdate", "Tour", new { notification = response.Description });

    }
    
    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
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
    

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
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
            .Include(t => t.Hotel).
            ThenInclude(t=>t.HotelProperties).
            ThenInclude(h=>h.BookingType).
            Include(t => t.Hotel).
            ThenInclude(t=>t.HotelProperties).
            ThenInclude(t=>t.HotelServices).Include(t => t.Route).
            Include(t => t.TourType);
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
        review.IsAccept = false; 
        review.User = _userRepository.getAll().FirstOrDefault(u => u.Account.Login == User.Identity.Name);
        await _reviewRepository.addReview(review);

        return RedirectToAction("TourInfo", "Tour", new { id = review.TourId });
    }

    [Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
    public async Task<IActionResult> TourToCsv()
    {
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, System.Text.Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        await writer.WriteLineAsync("Tour");
        await csv.WriteRecordsAsync(await _tourRepository.getAll().ToListAsync());
        await csv.NextRecordAsync();

           await writer.FlushAsync();
                    ms.Seek(0, SeekOrigin.Begin);
                    var result = ms.ToArray();
                    ms.Close();
                    return File(result, "application/force-download", "tours.csv");
    }

    public async Task<IActionResult> TourBookingServiceAdd(TourBookingViewModel tourBookingViewModel)
    {
        TourBooking tourBooking = await _tourBookingRepository.getQuery()
            .SingleOrDefaultAsync(t => t.TourId == (int)TempData["tourId"]);
        
        tourBooking.HotelServices.Add(await _hotelService.getHotelService(tourBookingViewModel.ServiceId));
       
        _tourBookingRepository.updateTourBooking(tourBooking);

        return RedirectToAction("TourBooking", "Tour", new { id = (int)TempData["tourId"] });
    }

    public async Task<IActionResult> TourBookingServiceRemove(int id)
    {
        TourBooking tourBooking = await _tourBookingRepository.getQuery().Include(t=>t.HotelServices)
            .SingleOrDefaultAsync(t => t.TourId == (int)TempData["tourId"]);
        
        tourBooking.HotelServices.Remove(await _hotelService.getHotelService(id));
       
        _tourBookingRepository.updateTourBooking(tourBooking);
        
        return RedirectToAction("TourBooking", "Tour", new { id = (int)TempData["tourId"] });
    }

    public async Task<IActionResult> MakeTourBooking(TourBookingViewModel model)
    {
        TourBooking tourBooking = await _tourBookingRepository.getQuery().Include(t=>t.HotelServices)
            .SingleOrDefaultAsync(t => t.TourId == (int)TempData["tourId"]);
        tourBooking.IsConfirmed = false;
        tourBooking.IsOnModerate = true;

        tourBooking.BookingTime = DateTime.Now;
        tourBooking.ArrivalTime = model.ArrivalTime;
        tourBooking.SleepingPlaceValue = model.SleepingPlaceValue; 
        
        _tourBookingRepository.updateTourBooking(tourBooking);

        return RedirectToAction("Main", "Home",new { notification = "Please, wait while our personal accept your request" });
    } 
    
    public async Task<IActionResult> TourBooking(int id)
    {
        bool isTourBookingExist = false;
        Tour tour = await _tourRepository.getAll().Include(t => t.Hotel).ThenInclude(t => t.HotelProperties)
            .ThenInclude(t => t.HotelServices).Where(t=>t.Id == id).SingleOrDefaultAsync();
        
        
        
        TourBookingViewModel model = new TourBookingViewModel();
        model.AllService = new(tour.Hotel.HotelProperties.HotelServices, nameof(HotelService.Id), nameof(HotelService.Name));



        var user = await _userRepository.getAll().Include(u => u.TourBookings).ThenInclude(t=>t.HotelServices).Include(u=>u.Account).SingleOrDefaultAsync(u=>u.Account.Login == User.Identity.Name);

        
        foreach (var tourBooking in user.TourBookings)
        {
            if (tourBooking.TourId == id);
            {
                model.SelectedServices = tourBooking.HotelServices;
                isTourBookingExist = true;

                if (tourBooking.IsOnModerate)
                    return RedirectToAction("Cart", "Cart", new { notification = "Sorry, you request to book this is in proccessing now!" });
                break; 
            }
        }

        if (!isTourBookingExist)
        {
            await _tourBookingRepository.addTourBooking(new TourBooking()
            {
                UserId = user.Id, TourId = id
            }); 
        }
        
        

        foreach (var service in model.SelectedServices)
            model.TotalServiceCost += service.Cost;

        model.CostForBed = tour.Hotel.CostForBed; 
        
        TempData["tourId"] = id;
        
        return View(model); 
    }
}