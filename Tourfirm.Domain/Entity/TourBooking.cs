using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class TourBooking
{
    [Key]
    public int Id { get; set; }
    public DateTime BookingTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    
    public int TourId { get; set; }
    public Tour? Tour { get; set; }

    public List<HotelService> HotelServices { get; set; } = new();   
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int  SleepingPlaceValue { get; set; }
    
    public bool IsConfirmed { get; set; }
    
}