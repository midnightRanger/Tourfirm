using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class TourBookingRepository : ITourBooking
{
    private readonly ApplicationContext _db;
    
    public TourBookingRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addTourBooking(TourBooking tourBooking)
    {
        _db.TourBooking.Add(tourBooking);
        await _db.SaveChangesAsync();
    }

    public void updateTourBooking(TourBooking tourBooking)
    {
        _db.Entry(tourBooking).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public TourBooking deleteTourBooking(in int id)
    {
        TourBooking? tourBooking = _db.TourBooking.Find(id);

        if (tourBooking != null)
        {
            _db.TourBooking.Remove(tourBooking);
            _db.SaveChanges();
            return tourBooking;
        }

        throw new ArgumentNullException();
    }

    public bool checkTourBooking(int id)
    {
        return _db.TourBooking.Any(r => r.Id == id);
    }

    public async Task<List<TourBooking>> getTourBookings()
    {
        return await _db.TourBooking.ToListAsync();
    }

    public async Task<TourBooking> getTourBooking(int id)
    {
        TourBooking? tourBooking = await _db.TourBooking.FindAsync(id);

        if (tourBooking != null)
        {
            return tourBooking;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<TourBooking> getQuery()
    {
        return _db.TourBooking; 
    }
}