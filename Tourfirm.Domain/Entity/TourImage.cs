using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class TourImage
{
    [Key]
    public int Id { get; set; }
    public string? Path { get; set; }
    
    public int TourId { get; set; }
    public Tour? Tour { get; set; }
}