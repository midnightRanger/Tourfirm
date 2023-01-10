using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Tour
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double Cost { get; set; }
    
    public int RouteId { get; set; }
    public Route? Route { get; set; }
    
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    
    public int TourTypeId { get; set; }
    public TourType? TourType { get; set; }
    
    public int CountryId { get; set; }
    public Country? Country { get; set; }

    public List<Cart> Carts { get; set; } = new();
    public List<TourImage> TourImages { get; set; } = new();
    public List<PaymentInfo> PaymentInfos { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}