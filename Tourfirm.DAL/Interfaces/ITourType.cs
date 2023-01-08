using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface ITourType
{
    public Task addTourType(TourType tourType);
    public void updateTourType(TourType tourType);
    public TourType deleteTourType(in int id);
    
    public bool checkTourType(int id);

    public Task<List<TourType>> getTourTypes();
    public Task<TourType> getTourType(int id);
    
    public IQueryable<TourType> getAll();
}