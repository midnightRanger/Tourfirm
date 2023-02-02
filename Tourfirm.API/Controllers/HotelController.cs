using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/hotel")]
[ApiController]
//Контроллер для работы с апи, данные - Отели
public class HotelController: ControllerBase
{
    private readonly IHotel _IHotel;

    public HotelController(IHotel iHotel)
    {
        _IHotel = iHotel;
    }
    //Получение элементов
    //get api/hotel
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hotel>>> Get()
    {
        return await Task.FromResult(await _IHotel.getHotels()); 
    }
    
    //Получение элемента
    //get api/hotel/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Hotel>> Get(int id)
    {
        var hotel = await Task.FromResult(await _IHotel.getHotel(id));
        if (hotel == null)
            return NotFound();
        return hotel; 
    }
    //Добавление элемента
    //post api/hotel
    [HttpPost]
    public async Task<ActionResult<Hotel>> Post(Hotel hotel)
    {
        await _IHotel.addHotel(hotel);
        return await Task.FromResult(hotel);
    }
    
    //Обновление элемента
    // PUT api/hotel/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Hotel>> Put(int id, Hotel hotel)
    {
        if (id != hotel.Id)
        {
            return BadRequest();
        }
        try
        {
            _IHotel.updateHotel(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HotelExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(hotel);
    }
    //Удаление элемента
    // DELETE api/hotel/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Hotel>> Delete(int id)
    {
        var hotel = _IHotel.deleteHotel(id);
        return await Task.FromResult(hotel);
    }
    
    //Существует ли
    private bool HotelExists(int id)
    {
        return _IHotel.checkHotel(id);
    }
    
    
}
