using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tourfirm.Domain.ViewModels;

public class TourAddViewModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double Cost { get; set; }

    public SelectList? AllRoutes { get; set; }
    public SelectList? AllHotels { get; set; }
    public SelectList? AllTourTypes { get; set; }
    public SelectList AllCountries { get; set; }
    
    [Required(ErrorMessage = "Please select files")]
    public List<IFormFile> Files { get; set; }
}