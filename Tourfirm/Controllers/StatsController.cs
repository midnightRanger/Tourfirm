using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tourfirm.DAL.Interfaces;

namespace Tourfirm.Controllers;

public class StatsController : Controller
{
    private readonly ITour _tourRepository;


    public StatsController(ITour tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<IActionResult> MainStats()
    {
        List<String> tourName = _tourRepository.getAll().Select(u => u.Name).ToList();
        List<Double> price = _tourRepository.getAll().Select(u => u.Cost).ToList();

        
        ViewBag.productNameList = JsonSerializer.Serialize(tourName);
        ViewBag.productPriceList = string.Join(",", price);


        return View(); 
    }
}