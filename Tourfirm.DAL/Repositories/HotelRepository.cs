using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class HotelRepository: IHotel
{
    private ApplicationContext _db = new();

    public HotelRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addHotel(Hotel hotel)
    {
        _db.Hotel.Add(hotel);
        await _db.SaveChangesAsync();
    }

    public void updateHotel(Hotel hotel)
    {
        _db.Entry(hotel).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Hotel deleteHotel(in int id)
    {
        Hotel? hotel = _db.Hotel.Find(id);

        if (hotel != null)
        {
            _db.Hotel.Remove(hotel);
            _db.SaveChanges();
            return hotel;
        }

        throw new ArgumentNullException();
    }

    public bool checkHotel(int id)
    {
        return _db.Hotel.Any(h => h.Id == id);
    }

    public async Task<List<Hotel>> getHotels()
    {
        return await _db.Hotel.ToListAsync();
    }

    public async Task<Hotel> getHotel(int id)
    {
        Hotel? hotel = await _db.Hotel.FindAsync(id);

        if (hotel != null)
        {
            return hotel;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<Hotel> getAll()
    {
        return _db.Hotel; 
    }
}