using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class PaymentInfo
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Cost { get; set; }
    
    public int TourId { get; set; }
    public Tour? Tour { get; set; }
}