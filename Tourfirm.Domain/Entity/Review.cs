using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Review
{
    [Key]
    public int Id { get; set; }
    public string? Text { get; set; }
    public bool IsRecommend { get; set; }
    public bool IsAccept { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int TourId { get; set; }
    public Tour? Tour { get; set; }
}