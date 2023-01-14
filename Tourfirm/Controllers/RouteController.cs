using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Route = Tourfirm.Domain.Entity.Route;

namespace Tourfirm.Controllers;

public class RouteController : Controller
{
    private readonly ILogger<RouteController> _logger;
    private readonly ApplicationContext _db;
    private readonly IRoute _routeRepository;

    public RouteController(ApplicationContext db, ILogger<RouteController> logger, IRoute routeRepository)
    {
        _db = db;
        _logger = logger;
        _routeRepository = routeRepository;
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