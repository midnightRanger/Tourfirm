using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;
using Route = Tourfirm.Domain.Entity.Route;

namespace Tourfirm.Controllers;

public class RouteController : Controller
{
    private readonly ILogger<RouteController> _logger;
    private readonly ApplicationContext _db;
    private readonly IRoute _routeRepository;
    private readonly IRouteService _routeService; 

    public RouteController(ApplicationContext db, ILogger<RouteController> logger, IRoute routeRepository, IRouteService routeService)
    {
        _db = db;
        _logger = logger;
        _routeRepository = routeRepository;
        _routeService = routeService;
    }

    [HttpGet]
    public IActionResult RouteAdd() => View();

    [HttpPost]
    public async Task<IActionResult> RouteAdd(Route route)
    {
        if (!ModelState.IsValid)
        {
            return View(route);
        }

        var response = await _routeService.CreateRoute(route);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("RouteIndex", "Route", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("RouteIndex", "Route", new { notification = response.Description });
    }
    
    [HttpGet]
    public async Task<IActionResult> RouteDeleteConfirm(int id)
    {
        return View(await _routeRepository.getRoute(id));
    }

    
    public async Task<IActionResult> RouteDelete(int id)
    {
        var response = await _routeService.DeleteRoute(await _routeRepository.getRoute(id));

        return RedirectToAction("RouteIndex", "Route", new { notification = response.Description});
        
    }

    [HttpGet]
    public async Task<IActionResult> RouteIndex(string? notification, Route.SortState sortRoute = Route.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdSort"] = sortRoute == Route.SortState.IdAsc ? Route.SortState.IdDesc : Route.SortState.IdAsc;
        IQueryable<Route> routes = _routeRepository.getAll();
        
        switch (sortRoute)
        {
            case Route.SortState.IdAsc:
            {
                routes = routes.OrderBy(p => p.Id);
                break;
            }

            case Route.SortState.IdDesc:
            {
                routes = routes.OrderByDescending(p => p.Id);
                break;
            }
        }

        return View(await routes.AsNoTracking().ToListAsync());
    }
}