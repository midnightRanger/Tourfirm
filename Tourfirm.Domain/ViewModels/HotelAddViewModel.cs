using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tourfirm.Domain.ViewModels;

public class HotelAddViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Fill the name field!")]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string? Name { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number, > 0")]
    public int Capacity { get; set; }
    [Range(0, 5, ErrorMessage = "Please enter valid integer Number, 5 >= stars >= 0")]
    public int Stars { get; set; }
    [Required(ErrorMessage = "Fill the Classification field!")]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string? Classification { get; set; }
    [Required(ErrorMessage = "Fill the food type field!")]
    [DataType(DataType.Text)]
    [Display(Name="Food")]
    
    public int? BookingTypeId { get; set; }
    public string? Food { get; set; }
    public SelectList? AllBookings { get; set; }
    public string? Style { get; set; }
}