using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class HotelPropertiesRepository : IHotelProperties
{
    private ApplicationContext _db = new();

    public HotelPropertiesRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addHotelProperties(HotelProperties hotelProperties)
    {
        _db.HotelProperties.Add(hotelProperties);
        await _db.SaveChangesAsync();
    }

    public void updateHotelProperties(HotelProperties hotelProperties)
    {
        _db.Entry(hotelProperties).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public HotelProperties deleteHotelProperties(in int id)
    {
        HotelProperties? hotelProperties = _db.HotelProperties.Find(id);

        if (hotelProperties != null)
        {
            _db.HotelProperties.Remove(hotelProperties);
            _db.SaveChanges();
            return hotelProperties;
        }

        throw new ArgumentNullException();
    }

    public bool checkHotelProperties(int id)
    {
        return _db.HotelProperties.Any(h => h.Id == id);
    }

    public async Task<List<HotelProperties>> getHotelProperties()
    {
        return await _db.HotelProperties.ToListAsync();
    }

    public async Task<HotelProperties> getHotelProperty(int id)
    {
        HotelProperties? hotelProperties = await _db.HotelProperties.FindAsync(id);

        if (hotelProperties != null)
        {
            return hotelProperties;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<HotelProperties> getAll()
    {
        return _db.HotelProperties; 
    }
}