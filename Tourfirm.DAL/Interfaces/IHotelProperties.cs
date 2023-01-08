using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface IHotelProperties
{
    public Task addHotelProperties(HotelProperties hotelProperties);
    public void updateHotelProperties(HotelProperties hotelProperties);
    public HotelProperties deleteHotelProperties(in int id);
    
    public bool checkHotelProperties(int id);

    public Task<List<HotelProperties>> getHotelProperties();
    public Task<HotelProperties> getHotelProperty(int id);
    
    public IQueryable<HotelProperties> getAll(); 
}