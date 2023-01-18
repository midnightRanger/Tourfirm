using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class HotelProperties
{
    [Key]
    public int Id { get; set; }
    public int Capacity { get; set; }
    public int Stars { get; set; }
    public string? Classification { get; set; }
    public string? Food { get; set; }
    
    public int? BookingTypeId { get; set; }
    public BookingType? BookingType { get; set; }
    public string? Style { get; set; }
    
    public Hotel? Hotel { get; set; }

    public List<HotelService> HotelServices { get; set; } = new();
}