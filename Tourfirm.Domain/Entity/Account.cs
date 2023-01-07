using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Account
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool? isActive { get; set; }
    public List<Role> Roles { get; set; } = new();
    public User? User { get; set; }
}