using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface ITourBooking
{
    public Task addTourBooking(TourBooking tourBooking);
    public void updateTourBooking(TourBooking tourBooking);
    public TourBooking deleteTourBooking(in int id);
    
    public bool checkTourBooking(int id);

    public Task<List<TourBooking>> getTourBookings();
    public Task<TourBooking> getTourBooking(int id);
    
    public IQueryable<TourBooking> getQuery(); 
}