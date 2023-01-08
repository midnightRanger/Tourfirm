using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class HotelServiceRepository : IHotelService
{
    private ApplicationContext _db = new();

    public HotelServiceRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addHotelService(HotelService hotelService)
    {
        _db.HotelService.Add(hotelService);
        await _db.SaveChangesAsync();
    }

    public void updateHotelService(HotelService hotelService)
    {
        _db.Entry(hotelService).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public HotelService deleteHotelService(in int id)
    {
        HotelService? hotelService = _db.HotelService.Find(id);

        if (hotelService != null)
        {
            _db.HotelService.Remove(hotelService);
            _db.SaveChanges();
            return hotelService;
        }

        throw new ArgumentNullException();
    }

    public bool checkHotelService(int id)
    {
        return _db.HotelService.Any(h => h.Id == id);
    }

    public async Task<List<HotelService>> getHotelServices()
    {
        return await _db.HotelService.ToListAsync();
    }

    public async Task<HotelService> getHotelService(int id)
    {
        HotelService? hotelService = await _db.HotelService.FindAsync(id);

        if (hotelService != null)
        {
            return hotelService;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<HotelService> getAll()
    {
        return _db.HotelService; 
    }
}