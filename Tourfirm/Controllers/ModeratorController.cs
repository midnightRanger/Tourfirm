using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.ViewModels;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace Tourfirm.Controllers;
[Authorize(Roles="ADMIN,MODERATOR")]
public class ModeratorController : Controller
{
    private readonly IReview _reviewRepository;
    private readonly IUser _userRepository;
    private readonly IRole _roleRepository;
    private readonly ApplicationContext _db;

    public ModeratorController(IReview reviewRepository, IUser userRepository, IRole roleRepository, ApplicationContext db)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _db = db;
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

        IQueryable<User> users = _userRepository.getAll().Include(u=>u.Account).ThenInclude(a=>a.Roles);
        
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
    
    [HttpGet]
    public async Task<IActionResult> UserRoleUpdate(string? notification)
    {
        User? user = _userRepository.getAll().Include(u=>u.Account).ThenInclude(a=>a.Roles).FirstOrDefault(u => u.Account.Login == User.Identity.Name);

        if(notification != null)
            ModelState.AddModelError("", notification);

        UserRoleUpdateViewModel roleModel = new UserRoleUpdateViewModel();
        roleModel.UserRoles = user.Account.Roles; 
        roleModel.AllRoles = new(await _roleRepository.getRoles(), nameof(Role.Id), nameof(Role.Name));

        return View(roleModel);
    }

    [HttpPost]
    public async Task<IActionResult> UserRoleUpdate(UserRoleUpdateViewModel roleModel)
    {
        User? user = _userRepository.getAll().Include(u=>u.Account).ThenInclude(a=>a.Roles).FirstOrDefault(u => u.Account.Login == User.Identity.Name);

        Role? role = await _roleRepository.getRole(roleModel.Id);
        user.Account.Roles.Add(role);
        await _db.SaveChangesAsync(); 
        
        return RedirectToAction("UserRoleUpdate", "Moderator", new { notification = "User roles was updated" });

    }
    
    public async Task<IActionResult> RoleRemove(int id)
    {
        User? user = _userRepository.getAll().Include(u=>u.Account).ThenInclude(a=>a.Roles).FirstOrDefault(u => u.Account.Login == User.Identity.Name);
        Role? role = await _roleRepository.getRole(id);
        user.Account.Roles.Remove(role);
        
        await _db.SaveChangesAsync(); 
        
        return RedirectToAction("UserRoleUpdate", "Moderator", new {notification = "Role was successfully removed! " });
    }
    
}