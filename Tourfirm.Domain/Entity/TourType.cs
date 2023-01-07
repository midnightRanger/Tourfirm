using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class TourType
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    
    public List<Tour> Tours { get; set; } = new();
}