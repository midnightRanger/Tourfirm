using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface ITourImage
{
    public Task addTourImage(TourImage tourImage);
    public void updateTourImage(TourImage tourImage);
    public TourImage deleteTourImage(in int id);
    
    public bool checkTourImage(int id);

    public Task<List<TourImage>> getTourImages();
    public Task<TourImage> getTourImage(int id);
    
    public IQueryable<TourImage> getAll();
}