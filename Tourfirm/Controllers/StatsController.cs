using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Controllers;

public class StatsController : Controller
{
    private readonly ITour _tourRepository;
    private readonly ICountry _countryRepository;
    private readonly IRole _roleRepository;
    public StatsController(ITour tourRepository, ICountry countryRepository, IRole roleRepository)
    {
        _tourRepository = tourRepository;
        _countryRepository = countryRepository;
        _roleRepository = roleRepository;
    }

    public async Task<IActionResult> MainStats()
    {
        List<String> tourName = _tourRepository.getAll().Select(u => u.Name).ToList();
        List<Double> price = _tourRepository.getAll().Select(u => u.Cost).ToList();

        ViewBag.productNameList = JsonSerializer.Serialize(tourName);
        ViewBag.productPriceList = string.Join(",", price);

        List<string?> countries = _countryRepository.getAll().Select(c => c.Name).ToList();
        List<int> countryTours = _countryRepository.getAll().Include(c => c.Tours).Select(c => c.Tours.Count).ToList();

        ViewBag.countryNames = JsonSerializer.Serialize(countries);
        ViewBag.tourValues = string.Join(",", countryTours);

        List<int> inCart = _tourRepository.getAll().Include(t => t.Carts).Select(c => c.Carts.Count).ToList();
        ViewBag.inCartValues = string.Join(",", inCart);

        List<string?> userRoles = _roleRepository.getAll().Select(r => r.Name).ToList();
        List<int> userRolesValue =
            _roleRepository.getAll().Include(r => r.Accounts).Select(r => r.Accounts.Count).ToList();

        ViewBag.userRoles = JsonSerializer.Serialize(userRoles);
        ViewBag.userRolesValue = string.Join(",", userRolesValue);
        return View();
    }
}