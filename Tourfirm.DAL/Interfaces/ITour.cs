using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface ITour
{
    public Task addTour(Tour tour);
    public void updateTour(Tour tour);
    public Tour deleteTour(in int id);
    
    public bool checkTour(int id);

    public Task<List<Tour>> getTours();
    public Task<Tour> getTour(int id);
    
    public IQueryable<Tour> getAll(); 
}