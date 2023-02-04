using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;
//интерфейс для CRUD функций с услугами отеля
public interface IHotelService
{
    public Task addHotelService(HotelService hotel);
    public void updateHotelService(HotelService hotel);
    public HotelService deleteHotelService(in int id);
    
    public bool checkHotelService(int id);

    public Task<List<HotelService>> getHotelServices();
    public Task<HotelService> getHotelService(int id);
    
    public IQueryable<HotelService> getAll(); 
}