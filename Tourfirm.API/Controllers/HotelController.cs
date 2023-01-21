using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/hotel")]
[ApiController]
public class HotelController: ControllerBase
{
    private readonly IHotel _IHotel;
    
    
    public HotelController(IHotel iHotel)
    {
        _IHotel = iHotel;
    }
    //get api/hotel
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hotel>>> Get()
    {
        return await Task.FromResult(await _IHotel.getHotels()); 
    }
    
    //get api/hotel/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Hotel>> Get(int id)
    {
        var hotel = await Task.FromResult(await _IHotel.getHotel(id));
        if (hotel == null)
            return NotFound();
        return hotel; 
    }
    
    //post api/hotel
    [HttpPost]
    public async Task<ActionResult<Hotel>> Post(Hotel hotel)
    {
        await _IHotel.addHotel(hotel);
        return await Task.FromResult(hotel);
    }
    
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
    // DELETE api/hotel/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Hotel>> Delete(int id)
    {
        var hotel = _IHotel.deleteHotel(id);
        return await Task.FromResult(hotel);
    }
    
    private bool HotelExists(int id)
    {
        return _IHotel.checkHotel(id);
    }
    
    
}
