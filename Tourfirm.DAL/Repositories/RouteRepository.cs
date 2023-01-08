using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class RouteRepository : IRoute
{
    private readonly ApplicationContext _db;
    
    public RouteRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addRoute(Route route)
    {
        _db.Route.Add(route);
        await _db.SaveChangesAsync();
    }

    public void updateRoute(Route route)
    {
        _db.Entry(route).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Route deleteRoute(in int id)
    {
        Route? route = _db.Route.Find(id);

        if (route != null)
        {
            _db.Route.Remove(route);
            _db.SaveChanges();
            return route;
        }

        throw new ArgumentNullException();
    }

    public bool checkRoute(int id)
    {
        return _db.Route.Any(r => r.Id == id);
    }

    public async Task<List<Route>> getRoutes()
    {
        return await _db.Route.ToListAsync();
    }

    public async Task<Route> getRoute(int id)
    {
        Route? route = await _db.Route.FindAsync(id);

        if (route != null)
        {
            return route;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<Route> getAll()
    {
        return _db.Route; 
    }
}