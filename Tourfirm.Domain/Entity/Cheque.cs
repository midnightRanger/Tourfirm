using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Cheque
{
    [Key]
    public int Id { get; set; }
    public double Sum { get; set; }
    public DateTime DateTime { get; set; }
    public int TourId { get; set; }
    public Tour? Tour { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
}