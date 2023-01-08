using System.ComponentModel.DataAnnotations;

namespace Tourfirm.DAL.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please, insert your email ")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please, insert your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
}