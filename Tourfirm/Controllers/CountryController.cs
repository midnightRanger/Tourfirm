using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;

[Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
public class CountryController : Controller
{
    private readonly ILogger<CountryController> _logger;
    private readonly ICountry _countryRepository;
    private readonly ICountryService _countryService;


    public CountryController(ILogger<CountryController> logger, ICountry countryRepository, ICountryService countryService)
    {
        _logger = logger;
        _countryRepository = countryRepository;
        _countryService = countryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> CountryIndex(string? notification, Country.SortState sortCountry = Country.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdSort"] = sortCountry == Country.SortState.IdAsc ? Country.SortState.IdDesc : Country.SortState.IdAsc;
        ViewData["NameSort"] = sortCountry == Country.SortState.NameAsc ? Country.SortState.NameDesc : Country.SortState.NameAsc;
        ViewData["LanguageSort"] = sortCountry == Country.SortState.LanguageAsc ? Country.SortState.LanguageDesc : Country.SortState.LanguageAsc;
        ViewData["MidTempSort"] = sortCountry == Country.SortState.MidTempAsc ? Country.SortState.MidTempDesc : Country.SortState.MidTempAsc;
        ViewData["ClimateSort"] = sortCountry == Country.SortState.ClimateAsc ? Country.SortState.ClimateDesc : Country.SortState.ClimateAsc;
         
        IQueryable<Country> countries = _countryRepository.getAll();
        
        switch (sortCountry)
        {
            case Country.SortState.IdAsc:
            {
                countries = countries.OrderBy(p => p.Id);
                break;
            }

            case Country.SortState.IdDesc:
            {
                countries = countries.OrderByDescending(p => p.Id);
                break;
            }
            case Country.SortState.ClimateAsc:
            {
                countries = countries.OrderBy(p => p.Climate);
                break;
            }
            case Country.SortState.ClimateDesc:
            {
                countries = countries.OrderByDescending(p => p.Climate);
                break;
            }
            case Country.SortState.LanguageAsc:
            {
                countries = countries.OrderBy(p => p.Language);
                break;
            }
            case Country.SortState.LanguageDesc:
            {
                countries = countries.OrderByDescending(p => p.Language);
                break;
            }
            case Country.SortState.MidTempAsc:
            {
                countries = countries.OrderBy(p => p.MidTemp);
                break;
            }
            case Country.SortState.MidTempDesc:
            {
                countries = countries.OrderByDescending(p => p.MidTemp);
                break;
            }
            
            case Country.SortState.NameAsc:
            {
                countries = countries.OrderBy(p => p.Name);
                break;
            }
            case Country.SortState.NameDesc:
            {
                countries = countries.OrderByDescending(p => p.Name);
                break;
            }
        }

        return View("CountryIndex", await countries.AsNoTracking().ToListAsync());
    }
}