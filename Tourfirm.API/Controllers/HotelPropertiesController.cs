using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/hotelproperties")]
[ApiController]
public class HotelPropertiesController: ControllerBase
{
    private readonly IHotelProperties _IHotelProperties;
    
    
    
    public HotelPropertiesController(IHotelProperties iHotelProperties)
    {
        _IHotelProperties = iHotelProperties;
    }
    //get api/hotelproperties
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelProperties>>> Get()
    {
        return await Task.FromResult(await _IHotelProperties.getHotelProperties()); 
    }
    
    //get api/hotelproperties/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelProperties>> Get(int id)
    {
        var hotelProperties = await Task.FromResult(await _IHotelProperties.getHotelProperty(id));
        if (hotelProperties == null)
            return NotFound();
        return hotelProperties; 
    }
    
    //post api/hotelproperties
    [HttpPost]
    public async Task<ActionResult<HotelProperties>> Post(HotelProperties hotelProperties)
    {
        await _IHotelProperties.addHotelProperties(hotelProperties);
        return await Task.FromResult(hotelProperties);
    }
    
    // PUT api/hotelproperties/5
    [HttpPut("{id}")]
    public async Task<ActionResult<HotelProperties>> Put(int id, HotelProperties hotelProperties)
    {
        if (id != hotelProperties.Id)
        {
            return BadRequest();
        }
        try
        {
            _IHotelProperties.updateHotelProperties(hotelProperties);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HotelPropertiesExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(hotelProperties);
    }
    // DELETE api/hotelproperties/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<HotelProperties>> Delete(int id)
    {
        var hotelProperties = _IHotelProperties.deleteHotelProperties(id);
        return await Task.FromResult(hotelProperties);
    }
    
    private bool HotelPropertiesExists(int id)
    {
        return _IHotelProperties.checkHotelProperties(id);
    }
    
    
}
