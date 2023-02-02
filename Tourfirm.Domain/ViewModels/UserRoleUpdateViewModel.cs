using Microsoft.AspNetCore.Mvc.Rendering;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Domain.ViewModels;

public class UserRoleUpdateViewModel
{
    public int Id {get; set; }
    public int UserId { get; set; }
    public SelectList? AllRoles { get; set; }
    public List<Role> UserRoles { get; set; } = new List<Role>(); 
}