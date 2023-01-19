using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;

public class TourTypeController: Controller
{
     private readonly ILogger<RouteController> _logger;
    private readonly ApplicationContext _db;
    private readonly ITourType _tourTypeRepository;
    private readonly ITourTypeService _tourTypeService;

    public TourTypeController(ILogger<RouteController> logger, ApplicationContext db, ITourType tourTypeRepository, ITourTypeService tourTypeService)
    {
        _logger = logger;
        _db = db;
        _tourTypeRepository = tourTypeRepository;
        _tourTypeService = tourTypeService;
    }

    [HttpGet]
    public IActionResult TourTypeAdd() => View();

    [HttpPost]
    public async Task<IActionResult> TourTypeAdd(TourType tourType)
    {
        if (!ModelState.IsValid)
        {
            return View(tourType);
        }

        var response = await _tourTypeService.CreateTourType(tourType);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("TourTypeIndex", "TourType", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("TourTypeIndex", "TourType", new { notification = response.Description });
    }
    
    [HttpGet]
    public async Task<IActionResult> TourTypeDeleteConfirm(int id)
    {
        return View(await _tourTypeRepository.getTourType(id));
    }

    
    public async Task<IActionResult> TourTypeDelete(int id)
    {
        var response = await _tourTypeService.DeleteTourType(await _tourTypeRepository.getTourType(id));

        return RedirectToAction("TourTypeIndex", "TourType", new { notification = response.Description});
        
    }

    [HttpGet]
    public async Task<IActionResult> TourTypeIndex(string? notification, TourType.SortState sortTourType = TourType.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdSort"] = sortTourType == TourType.SortState.IdAsc ? TourType.SortState.IdDesc : TourType.SortState.IdAsc;
        ViewData["NameSort"] = sortTourType == TourType.SortState.NameAsc ? TourType.SortState.NameDesc : TourType.SortState.NameAsc;

        IQueryable<TourType> tourTypes = _tourTypeRepository.getAll();
        
        switch (sortTourType)
        {
            case TourType.SortState.IdAsc:
            {
                tourTypes = tourTypes.OrderBy(p => p.Id);
                break;
            }

            case TourType.SortState.IdDesc:
            {
                tourTypes = tourTypes.OrderByDescending(p => p.Id);
                break;
            }
            
            case TourType.SortState.NameAsc:
            {
                tourTypes = tourTypes.OrderBy(p => p.Name);
                break;
            }

            case TourType.SortState.NameDesc:
            {
                tourTypes = tourTypes.OrderByDescending(p => p.Name);
                break;
            }
        }

        return View(await tourTypes.AsNoTracking().ToListAsync());
    }
    
     [HttpGet]
    public async Task<IActionResult> TourTypeUpdate(string? notification, int id)
    {
        var tourType = await _tourTypeRepository.getTourType(id);
        
        if(notification != null)
            ModelState.AddModelError("", notification);

        return View(tourType);
    }

    [HttpPost]
    public async Task<IActionResult> TourTypeUpdate(TourType tourType)
    {
        if (!ModelState.IsValid)
        {
            return View(tourType);
        }

        var response = await _tourTypeService.UpdateTourType(tourType);

        if (response.StatusCode == Domain.Safety.StatusCode.OK)
        {
            return RedirectToAction("TourTypeIndex", "TourType", new { notification = response.Description });
        }
        ModelState.AddModelError("", response.Description);
        return RedirectToAction("TourTypeIndex", "TourType", new { notification = response.Description });

    }
}