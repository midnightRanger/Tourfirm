using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Models;

namespace Tourfirm.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IEnumerable<Tour> _allTours;
    private readonly ITour _tourRepository; 

    public HomeController(ILogger<HomeController> logger, ITour tourRepository)
    {
        _logger = logger;
        _tourRepository = tourRepository;
        _allTours = _tourRepository.getAll().Include(t => t.Country).Include(t => t.TourType)
            .Include(t => t.Route).Include(t=>t.TourImages).Include(t => t.Hotel).ThenInclude(h => h.HotelProperties)
            .ThenInclude(h => h.HotelServices);
    }
    public IActionResult Main() => View();


    public IActionResult Index()
    {
        return View();
    }
    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> Main(string? keyword, string? notification)
    {
        ViewData["keyword"] = keyword;
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        IEnumerable<Tour> tourList;
        
        if (!String.IsNullOrEmpty(keyword))
            tourList = _allTours.Where(t
                => t.Name.Contains(keyword));
        else tourList = _allTours;

        return View(tourList); 
    }
}