using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface IHotel
{
    public Task addHotel(Hotel hotel);
    public void updateHotel(Hotel hotel);
    public Hotel deleteHotel(in int id);
    
    public bool checkHotel(int id);

    public Task<List<Hotel>> getHotels();
    public Task<Hotel> getHotel(int id);
    
    public IQueryable<Hotel> getAll(); 
}