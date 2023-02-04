using Tourfirm.Domain.Entity;

namespace Tourfirm.Domain.ViewModels;
//добавление услуг отеля
public class HotelServiceAddViewModel
{
    public int HotelPropertiesId { get; set; }
    public int? HotelId { get; set; }
    public int HotelServiceId { get; set; }
    public String? Name { get; set; }
    public double Cost { get; set; }

    public List<HotelService>? HotelServices = new();
}