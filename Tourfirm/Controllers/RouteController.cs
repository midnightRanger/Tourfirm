using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;
using Route = Tourfirm.Domain.Entity.Route;

namespace Tourfirm.Controllers;

[Authorize(Roles = "ADMIN,MODERATOR,MANAGER")]
public class RouteController : Controller
{
    private readonly ILogger<RouteController> _logger;
    private readonly IRoute _routeRepository;
    private readonly IRouteService _routeService;

    public RouteController(ILogger<RouteController> logger, IRoute routeRepository, IRouteService routeService)
    {
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

        return RedirectToAction("RouteIndex", "Route", new { notification = response.Description });

    }

    [HttpGet]
    public async Task<IActionResult> RouteIndex(string? notification, Route.SortState sortRoute = Route.SortState.IdAsc)
    {
        if (notification != null)
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

        return View("RouteIndex", await routes.AsNoTracking().ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> RouteUpdate(string? notification, int id)
    {
        var route = await _routeRepository.getRoute(id);

        if (notification != null)
            ModelState.AddModelError("", notification);

        return View(route);
    }

    [HttpPost]
    public async Task<IActionResult> RouteUpdate(Route route)
    {
        if (!ModelState.IsValid)
        {
            return View(route);
        }

        var response = await _routeService.UpdateRoute(route);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("RouteIndex", "Route", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("RouteUpdate", "Route", new { notification = response.Description });

    }
}