using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/bookingtype")]
[ApiController]
public class BookingTypeController: ControllerBase
{
    private readonly IBookingType _IBookingType;
    
    public BookingTypeController(IBookingType iBookingType)
    {
        _IBookingType = iBookingType;
    }

    //get api/bookingtype
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingType>>> Get()
    {
        return await Task.FromResult(await _IBookingType.getBookingTypes()); 
    }
    
    //get api/bookingtype/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingType>> Get(int id)
    {
        var bookingType = await Task.FromResult(await _IBookingType.getBookingType(id));
        if (bookingType == null)
            return NotFound();
        return bookingType; 
    }
    
    //post api/bookingtype
    [HttpPost]
    public async Task<ActionResult<BookingType>> Post(BookingType bookingType)
    {
        await _IBookingType.addBookingType(bookingType);
        return await Task.FromResult(bookingType);
    }
    
    // PUT api/bookingtype/5
    [HttpPut("{id}")]
    public async Task<ActionResult<BookingType>> Put(int id, BookingType bookingType)
    {
        if (id != bookingType.Id)
        {
            return BadRequest();
        }
        try
        {
            _IBookingType.updateBookingType(bookingType);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingTypeExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(bookingType);
    }
    // DELETE api/bookingtype/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<BookingType>> Delete(int id)
    {
        var bookingType = _IBookingType.deleteBookingType(id);
        return await Task.FromResult(bookingType);
    }

    private bool BookingTypeExists(int id)
    {
        return _IBookingType.checkBookingType(id);
    }
}