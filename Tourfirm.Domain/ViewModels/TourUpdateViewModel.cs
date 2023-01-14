using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Domain.ViewModels;

public class TourUpdateViewModel
{
    public int? Id { get; set; }
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
    
    public List<IFormFile>? Files { get; set; }

    public List<TourImage>? Images { get; set; } = new(); 
}