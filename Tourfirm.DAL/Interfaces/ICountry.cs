using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;
//интерфейс для CRUD функций стран
public interface ICountry
{
    public Task addCountry(Country country);
    public void updateCountry(Country country);
    public Country deleteCountry(in int id);
    
    public bool checkCountry(int id);

    public Task<List<Country>> getCountries();
    public Task<Country> getCountry(int id);
    
    public IQueryable<Country> getAll(); 
}