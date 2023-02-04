using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class TourImageRepository: ITourImage
{
    private readonly ApplicationContext _db;
    
    public TourImageRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addTourImage(TourImage tourImage)
    {
        _db.TourImage.Add(tourImage);
        await _db.SaveChangesAsync();
    }

    public void updateTourImage(TourImage tourImage)
    {
        _db.Entry(tourImage).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public TourImage deleteTourImage(in int id)
    {
        TourImage? tourImage = _db.TourImage.Find(id);

        if (tourImage != null)
        {
            _db.TourImage.Remove(tourImage);
            _db.SaveChanges();
            return tourImage;
        }

        throw new ArgumentNullException();
    }

    public bool checkTourImage(int id)
    {
        return _db.TourImage.Any(r => r.Id == id);
    }

    public async Task<List<TourImage>> getTourImages()
    {
        return await _db.TourImage.ToListAsync();
    }

    public async Task<TourImage> getTourImage(int id)
    {
        TourImage? tourImage = await _db.TourImage.FindAsync(id);

        if (tourImage != null)
        {
            return tourImage;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<TourImage> getAll()
    {
        return _db.TourImage; 
    }
}