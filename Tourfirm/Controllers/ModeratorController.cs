using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;

public class ModeratorController : Controller
{
    private readonly IReview _reviewRepository;

    public ModeratorController(IReview reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ReviewIndex(string? notification, Review.SortState sortReview = Review.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdSort"] = sortReview == Review.SortState.IdAsc ? Review.SortState.IdDesc : Review.SortState.IdAsc;
        ViewData["TextSort"] = sortReview == Review.SortState.TextAsc ? Review.SortState.TextDesc : Review.SortState.TextAsc;

        IQueryable<Review> reviews = _reviewRepository.getAll().Include(r=>r.Tour).Include(r=>r.User);
        
        switch (sortReview)
        {
            case Review.SortState.IdAsc:
            {
                reviews = reviews.OrderBy(p => p.Id);
                break;
            }

            case Review.SortState.IdDesc:
            {
                reviews = reviews.OrderByDescending(p => p.Id);
                break;
            }
            case Review.SortState.TextAsc:
            {
                reviews = reviews.OrderBy(p => p.Text);
                break;
            }

            case Review.SortState.TextDesc:
            {
                reviews = reviews.OrderByDescending(p => p.Text);
                break;
            }
        }

        return View("ReviewIndex", await reviews.AsNoTracking().ToListAsync());
    }
}