using Microsoft.AspNetCore.Mvc.Rendering;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Domain.ViewModels;

public class TourBookingViewModel
{
    public double CostForBed { get; set; }
    public SelectList? AllService { get; set; }
    public List<HotelService> SelectedServices { get; set; } = new();
    public int ServiceId { get; set; } 
    public DateTime ArrivalTime { get; set; }
    
    public int SleepingPlaceValue { get; set; }
    
    public double TotalServiceCost { get; set; }

    public Tour? Tour { get; set; }
    
}