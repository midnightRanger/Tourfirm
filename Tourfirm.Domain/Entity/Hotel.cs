using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Hotel
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Rate { get; set; }
    
    public double CostForBed { get; set; }
    public int HotelPropertiesId { get; set; }
    public HotelProperties? HotelProperties { get; set; }

    public List<Tour>? Tours { get; set; } = new();
    
    public enum SortState
    {
        IdAsc,
        IdDesc,
        NameAsc,
        NameDesc
    }
}