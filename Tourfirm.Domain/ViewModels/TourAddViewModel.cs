using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tourfirm.Models;

namespace Tourfirm.Domain.ViewModels;
//добавление нового тура
public class TourAddViewModel : ResponseModel.ReponseModel
{
    [Required(ErrorMessage = "Fill the name field!")]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Fill the description field!")]
    [DataType(DataType.Text)]
    [Display(Name="Description")]
    public string? Description { get; set; }
    public double Cost { get; set; }

    public SelectList? AllRoutes { get; set; }
    public SelectList? AllHotels { get; set; }
    public SelectList? AllTourTypes { get; set; }
    public SelectList? AllCountries { get; set; }
    
    [Required(ErrorMessage = "Please select files")]
    public List<IFormFile> Files { get; set; }
}