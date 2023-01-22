using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;

public class ModeratorController : Controller
{
    private readonly IReview _reviewRepository;
    private readonly IUser _userRepository;

    public ModeratorController(IReview reviewRepository, IUser userRepository)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
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

    public async Task<IActionResult> ReviewChange(int id)
    {
        Review? review = await _reviewRepository.getReview(id);

        review.IsAccept = !review.IsAccept;
        _reviewRepository.updateReview(review);

        return RedirectToAction("ReviewIndex", "Moderator", new { notification = "Review status was updated" });
    }

    public async Task<IActionResult> ReviewDelete(int id)
    {
        _reviewRepository.deleteReview(id);
        return RedirectToAction("ReviewIndex", "Moderator", new { notification = "Review was deleted" });

    }
    
    [HttpGet]
    public async Task<IActionResult> UserIndex(string? notification, User.SortState sortUser = Domain.Entity.User.SortState.IdAsc)
    {
        if(notification != null)
            ModelState.AddModelError("", notification);
        
        // ReSharper disable once HeapView.BoxingAllocation
        ViewData["IdSort"] = sortUser == Domain.Entity.User.SortState.IdAsc ? Domain.Entity.User.SortState.IdDesc : Domain.Entity.User.SortState.IdAsc;
        ViewData["LoginSort"] = sortUser == Domain.Entity.User.SortState.LoginAsc ? Domain.Entity.User.SortState.LoginDesc : Domain.Entity.User.SortState.LoginAsc;

        IQueryable<User> users = _userRepository.getAll().Include(u=>u.Account);
        
        switch (sortUser)
        {
            case Domain.Entity.User.SortState.IdAsc:
            {
                users = users.OrderBy(p => p.Id);
                break;
            }

            case Domain.Entity.User.SortState.IdDesc:
            {
                users = users.OrderByDescending(p => p.Id);
                break;
            }
            case Domain.Entity.User.SortState.LoginAsc:
            {
                users = users.OrderBy(p => p.Account.Login);
                break;
            }

            case Domain.Entity.User.SortState.LoginDesc:
            {
                users = users.OrderByDescending(p => p.Account.Login);
                break;
            }
        }

        return View("UserIndex", await users.AsNoTracking().ToListAsync());
    }
    
    public async Task<IActionResult> UserChange(int id)
    {
        User? user = await _userRepository.getAll().Include(u => u.Account).FirstOrDefaultAsync(u => u.Id == id);

        user.Account.isActive = !user.Account.isActive;
        _userRepository.updateUser(user);

        return RedirectToAction("UserIndex", "Moderator", new { notification = "User status was updated" });
    }
}