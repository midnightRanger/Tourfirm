using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tourfirm.Domain.ViewModels;

public class TourAddGetViewModel
{
    public SelectList? AllRoutes { get; set; }
    public SelectList? AllHotels { get; set; }
    public SelectList? AllTourTypes { get; set; }
    public SelectList AllCountries { get; set; }
}