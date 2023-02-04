using System.ComponentModel.DataAnnotations;

namespace Tourfirm.DAL.ViewModels;
//модель для регистрации
public class RegisterViewModel
{
    [Required(ErrorMessage = "Fill the login field!")]
    [Display(Name="Login")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Fill the e-mail field!")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Fill the password field!")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
 
    [Required(ErrorMessage = "Repeat the password")]
    [Compare("Password", ErrorMessage = "Passwords are different! ")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string PasswordConfirm { get; set; }
    
    [Required(ErrorMessage = "Fill the name field!")]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Fill the surname field!")]
    [DataType(DataType.Text)]
    [Display(Name="Surname")]
    public string? Surname { get; set; }
    
    [Required(ErrorMessage = "Fill the age field!")]
    [Display(Name="Age")]
    [Range(17, 100, ErrorMessage = "Value must be between 17 and 100")]
    public int Age { get; set; }
}