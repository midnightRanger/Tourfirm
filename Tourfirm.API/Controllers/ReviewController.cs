using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/review")]
[ApiController]
public class ReviewController: ControllerBase
{
    private readonly IReview _IReview;
    
    public ReviewController(IReview iReview)
    {
        _IReview = iReview;
    }
    
    //get api/review
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get()
    {
        return await Task.FromResult(await _IReview.getReviews()); 
    }
    
    //get api/review/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> Get(int id)
    {
        var review = await Task.FromResult(await _IReview.getReview(id));
        if (review == null)
            return NotFound();
        return review; 
    }
    
    //post api/review
    [HttpPost]
    public async Task<ActionResult<Review>> Post(Review review)
    {
        await _IReview.addReview(review);
        return await Task.FromResult(review);
    }
    
    // PUT api/review/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Review>> Put(int id, Review review)
    {
        if (id != review.Id)
        {
            return BadRequest();
        }
        try
        {
            _IReview.updateReview(review);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(review);
    }
    // DELETE api/review/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Review>> Delete(int id)
    {
        var review = _IReview.deleteReview(id);
        return await Task.FromResult(review);
    }
    
    private bool ReviewExists(int id)
    {
        return _IReview.checkReview(id);
    }
    
    
}
