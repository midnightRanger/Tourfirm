using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Cart
{
    [Key]
    public int Id { get; set; }
    public int Value { get; set; }
    public double Sum { get; set; }

    public List<Tour> Tours { get; set; } = new();
}