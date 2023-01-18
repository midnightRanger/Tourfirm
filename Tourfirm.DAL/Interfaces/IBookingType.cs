using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface IBookingType
{
    public Task addBookingType(BookingType bookingType);
    public void updateBookingType(BookingType bookingType);
    public BookingType deleteBookingType(in int id);
    
    public bool checkBookingType(int id);

    public Task<List<BookingType>> getBookingTypes();
    public Task<BookingType> getBookingType(int id);
    
    public IQueryable<BookingType> getAll(); 
}