using Tourfirm.Domain.Entity;

namespace Tourfirm.Domain.ViewModels;

public class HotelServiceAddViewModel
{
    public int HotelPropertiesId { get; set; }
    public int? HotelId { get; set; }
    public int HotelServiceId { get; set; }
    public String? Name { get; set; }
    public double Cost { get; set; }

    public List<HotelService>? HotelServices = new();
}