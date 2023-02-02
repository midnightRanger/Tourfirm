using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/country")]
[ApiController]
//Контроллер для работы с апи, данные - Страны
public class CountryController: ControllerBase
{
    private readonly ICountry _ICountry;

    public CountryController(ICountry iCountry)
    {
        _ICountry = iCountry;
    }

    //Получение элемента
    //get api/route 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> Get()
    {
        return await Task.FromResult(await _ICountry.getCountries()); 
    }
    
    //Получение элементов
    //get api/country/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> Get(int id)
    {
        var country = await Task.FromResult(await _ICountry.getCountry(id));
        if (country == null)
            return NotFound();
        return country; 
    }
    
    //Добавление элемента
    //post api/country
    [HttpPost]
    public async Task<ActionResult<Country>> Post(Country country)
    {
        await _ICountry.addCountry(country);
        return await Task.FromResult(country);
    }
    
    //Обновление элемента
    // PUT api/country/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Country>> Put(int id, Country country)
    {
        if (id != country.Id)
        {
            return BadRequest();
        }
        try
        {
            _ICountry.updateCountry(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(country);
    }
    //Удаление элемента
    // DELETE api/country/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Country>> Delete(int id)
    {
        var country = _ICountry.deleteCountry(id);
        return await Task.FromResult(country);
    }
    //Существует ли
    private bool CountryExists(int id)
    {
        return _ICountry.checkCountry(id);
    }
}
