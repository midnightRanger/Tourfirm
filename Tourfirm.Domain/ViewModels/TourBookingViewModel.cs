using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tourfirm.Domain.ViewModels;

public class TourBookingViewModel
{
    public SelectList? AllService { get; set; }
}