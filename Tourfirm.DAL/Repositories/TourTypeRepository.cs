using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class TourTypeRepository : ITourType
{
    private readonly ApplicationContext _db;
    
    public TourTypeRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addTourType(TourType tourType)
    {
        _db.TourType.Add(tourType);
        await _db.SaveChangesAsync();
    }

    public void updateTourType(TourType tourType)
    {
        _db.Entry(tourType).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public TourType deleteTourType(in int id)
    {
        TourType? tourType = _db.TourType.Find(id);

        if (tourType != null)
        {
            _db.TourType.Remove(tourType);
            _db.SaveChanges();
            return tourType;
        }

        throw new ArgumentNullException();
    }

    public bool checkTourType(int id)
    {
        return _db.TourType.Any(r => r.Id == id);
    }

    public async Task<List<TourType>> getTourTypes()
    {
        return await _db.TourType.ToListAsync();
    }

    public async Task<TourType> getTourType(int id)
    {
        TourType? tourType = await _db.TourType.FindAsync(id);

        if (tourType != null)
        {
            return tourType;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<TourType> getAll()
    {
        return _db.TourType; 
    }
}