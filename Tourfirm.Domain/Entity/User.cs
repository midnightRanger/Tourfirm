using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int? Age { get; set; }
    public double? Balance { get; set; }
    public int AccountId { get; set; }
    public Account? Account { get; set;  }
    public List<Cheque> Cheques { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
    public List<TourBooking> TourBookings { get; set; } = new();
    
    public Cart? Cart { get; set; }
    
    public enum SortState
    {
        IdAsc,
        IdDesc,
        LoginAsc,
        LoginDesc
    }
}