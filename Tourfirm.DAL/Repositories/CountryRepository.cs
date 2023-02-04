using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class CountryRepository : ICountry
{
    private ApplicationContext _db = new();

    public CountryRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addCountry(Country country)
    {
        _db.Country.Add(country);
        await _db.SaveChangesAsync();
    }

    public void updateCountry(Country country)
    {
        _db.Entry(country).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Country deleteCountry(in int id)
    {
        Country? country = _db.Country.Find(id);

        if (country != null)
        {
            _db.Country.Remove(country);
            _db.SaveChanges();
            return country;
        }

        throw new ArgumentNullException();
    }

    public bool checkCountry(int id)
    {
        return _db.Country.Any(c => c.Id == id);
    }

    public async Task<List<Country>> getCountries()
    {
        return await _db.Country.ToListAsync();
    }

    public async Task<Country> getCountry(int id)
    {
        Country? country = await _db.Country.FindAsync(id);

        if (country != null)
        {
            return country;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<Country> getAll()
    {
        return _db.Country; 
    }
}