using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/hotelservice")]
[ApiController]
public class HotelServiceController: ControllerBase
{
    private readonly IHotelService _IHotelService;
    
    
    
    
    public HotelServiceController(IHotelService iHotelService)
    {
        _IHotelService = iHotelService;
    }
    //get api/hotelservice
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelService>>> Get()
    {
        return await Task.FromResult(await _IHotelService.getHotelServices()); 
    }
    
    //get api/hotel/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelService>> Get(int id)
    {
        var hotelService = await Task.FromResult(await _IHotelService.getHotelService(id));
        if (hotelService == null)
            return NotFound();
        return hotelService; 
    }
    
    //post api/hotelservice
    [HttpPost]
    public async Task<ActionResult<HotelService>> Post(HotelService hotelService)
    {
        await _IHotelService.addHotelService(hotelService);
        return await Task.FromResult(hotelService);
    }
    
    // PUT api/hotelservice/5
    [HttpPut("{id}")]
    public async Task<ActionResult<HotelService>> Put(int id, HotelService hotelService)
    {
        if (id != hotelService.Id)
        {
            return BadRequest();
        }
        try
        {
            _IHotelService.updateHotelService(hotelService);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HotelServiceExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(hotelService);
    }
    // DELETE api/hotelservice/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<HotelService>> Delete(int id)
    {
        var hotelService = _IHotelService.deleteHotelService(id);
        return await Task.FromResult(hotelService);
    }
    
    private bool HotelServiceExists(int id)
    {
        return _IHotelService.checkHotelService(id);
    }
    
    
}
