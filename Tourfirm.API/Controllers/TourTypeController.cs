using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/tourtype")]
[ApiController]
public class TourTypeController: ControllerBase
{
    private readonly ITourType _tourType;

    
    public TourTypeController(ITourType tourType)
    {
        _tourType = tourType;
    }

    //get api/tourtype 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourType>>> Get()
    {
        return await Task.FromResult(await _tourType.getTourTypes()); 
    }
    
    //get api/tourtype/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<TourType>> Get(int id)
    {
        var tourtype = await Task.FromResult(await _tourType.getTourType(id));
        if (tourtype == null)
            return NotFound();
        return tourtype; 
    }
    
    //post api/tourtype
    [HttpPost]
    public async Task<ActionResult<TourType>> Post(TourType tourtype)
    {
        await _tourType.addTourType(tourtype);
        return await Task.FromResult(tourtype);
    }
    
    // PUT api/tourtype/5
    [HttpPut("{id}")]
    public async Task<ActionResult<TourType>> Put(int id, TourType tourtype)
    {
        if (id != tourtype.Id)
        {
            return BadRequest();
        }
        try
        {
            _tourType.updateTourType(tourtype);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TourTypeExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(tourtype);
    }
    // DELETE api/route/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<TourType>> Delete(int id)
    {
        var tourtype = _tourType.deleteTourType(id);
        return await Task.FromResult(tourtype);
    }

    private bool TourTypeExists(int id)
    {
        return _tourType.checkTourType(id);
    }
}
