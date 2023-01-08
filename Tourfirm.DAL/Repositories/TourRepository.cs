using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class TourRepository: ITour
{
    
    private readonly ApplicationContext _db;
    
    public TourRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addTour(Tour tour)
    {
        _db.Tour.Add(tour);
        await _db.SaveChangesAsync();
    }

    public void updateTour(Tour tour)
    {
        _db.Entry(tour).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Tour deleteTour(in int id)
    {
        Tour? tour = _db.Tour.Find(id);

        if (tour != null)
        {
            _db.Tour.Remove(tour);
            _db.SaveChanges();
            return tour;
        }

        throw new ArgumentNullException();
    }

    public bool checkTour(int id)
    {
        return _db.Tour.Any(r => r.Id == id);
    }

    public async Task<List<Tour>> getTours()
    {
        return await _db.Tour.ToListAsync();
    }

    public async Task<Tour> getTour(int id)
    {
        Tour? tour = await _db.Tour.FindAsync(id);

        if (tour != null)
        {
            return tour;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<Tour> getAll()
    {
        return _db.Tour; 
    }
}