using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/tourtype")]
[ApiController]
public class TourImageController: ControllerBase
{
    private readonly ITourImage _tourImage;

    
    public TourImageController(ITourImage tourImage)
    {
        _tourImage = tourImage;
    }

    //get api/tourtype 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourImage>>> Get()
    {
        return await Task.FromResult(await _tourImage.getTourImages()); 
    }
    
    //get api/tourtype/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<TourImage>> Get(int id)
    {
        var tourtype = await Task.FromResult(await _tourImage.getTourImage(id));
        if (tourtype == null)
            return NotFound();
        return tourtype; 
    }
    
    //post api/tourtype
    [HttpPost]
    public async Task<ActionResult<TourImage>> Post(TourImage tourtype)
    {
        await _tourImage.addTourImage(tourtype);
        return await Task.FromResult(tourtype);
    }
    
    // PUT api/tourtype/5
    [HttpPut("{id}")]
    public async Task<ActionResult<TourImage>> Put(int id, TourImage tourtype)
    {
        if (id != tourtype.Id)
        {
            return BadRequest();
        }
        try
        {
            _tourImage.updateTourImage(tourtype);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TourImageExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(tourtype);
    }
    // DELETE api/route/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<TourImage>> Delete(int id)
    {
        var tourtype = _tourImage.deleteTourImage(id);
        return await Task.FromResult(tourtype);
    }

    private bool TourImageExists(int id)
    {
        return _tourImage.checkTourImage(id);
    }
}
