

using Tourfirm.Domain.Entity;
//интерфейс для CRUD функций  с маршрутами
namespace Tourfirm.DAL.Interfaces;

public interface IRoute
{
    public Task addRoute(Route route);
    public void updateRoute(Route route);
    public Route deleteRoute(in int id);
    
    public bool checkRoute(int id);

    public Task<List<Route>> getRoutes();
    public Task<Route> getRoute(int id);
    
    public IQueryable<Route> getAll(); 
}