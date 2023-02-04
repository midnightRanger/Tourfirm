using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class BookingRepository : IBookingType
{
    private readonly ApplicationContext _db;
    
    public BookingRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addBookingType(BookingType bookingType)
    {
        _db.BookingType.Add(bookingType);
        await _db.SaveChangesAsync();
    }

    public void updateBookingType(BookingType bookingType)
    {
        _db.Entry(bookingType).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public BookingType deleteBookingType(in int id)
    {
        BookingType? bookingType = _db.BookingType.Find(id);

        if (bookingType != null)
        {
            _db.BookingType.Remove(bookingType);
            _db.SaveChanges();
            return bookingType;
        }

        throw new ArgumentNullException();
    }

    public bool checkBookingType(int id)
    {
        return _db.BookingType.Any(r => r.Id == id);
    }

    public async Task<List<BookingType>> getBookingTypes()
    {
        return await _db.BookingType.ToListAsync();
    }

    public async Task<BookingType> getBookingType(int id)
    {
        BookingType? bookingType = await _db.BookingType.FindAsync(id);

        if (bookingType != null)
        {
            return bookingType;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<BookingType> getAll()
    {
        return _db.BookingType; 
    }
}