using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/tour")]
[ApiController]
public class TourController: ControllerBase
{
    private readonly ITour _tour;

    
    public TourController(ITour tour)
    {
        _tour= tour;
    }

    //get api/tourtype 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tour>>> Get()
    {
        return await Task.FromResult(await _tour.getTours()); 
    }
    
    //get api/tourtype/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Tour>> Get(int id)
    {
        var tourtype = await Task.FromResult(await _tour.getTour(id));
        if (tourtype == null)
            return NotFound();
        return tourtype; 
    }
    
    //post api/tourtype
    [HttpPost]
    public async Task<ActionResult<Tour>> Post(Tour tourtype)
    {
        await _tour.addTour(tourtype);
        return await Task.FromResult(tourtype);
    }
    
    // PUT api/tourtype/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Tour>> Put(int id, Tour tourtype)
    {
        if (id != tourtype.Id)
        {
            return BadRequest();
        }
        try
        {
            _tour.updateTour(tourtype);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TourExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(tourtype);
    }
    // DELETE api/route/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Tour>> Delete(int id)
    {
        var tourtype = _tour.deleteTour(id);
        return await Task.FromResult(tourtype);
    }

    private bool TourExists(int id)
    {
        return _tour.checkTour(id);
    }
}
