using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class HotelService
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Cost { get; set; }
    
    public int HotelPropertiesId { get; set; }
    public HotelProperties? HotelProperties { get; set; }
}