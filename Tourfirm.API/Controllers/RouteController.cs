using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/route")]
[ApiController]
public class RouteController: ControllerBase
{
    private readonly IRoute _iRoute;

    public RouteController( IRoute iRoute)
    {
        _iRoute = iRoute;
    }
    
    //get api/route 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Route>>> Get()
    {
        return await Task.FromResult(await _iRoute.getRoutes()); 
    }
    
    //get api/route/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Route>> Get(int id)
    {
        var route = await Task.FromResult(await _iRoute.getRoute(id));
        if (route == null)
            return NotFound();
        return route; 
    }
    
    //post api/route
    [HttpPost]
    public async Task<ActionResult<Route>> Post(Route route)
    {
        _iRoute.addRoute(route);
        return await Task.FromResult(route);
    }
    
    // PUT api/route/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Route>> Put(int id, Route route)
    {
        if (id != route.Id)
        {
            return BadRequest();
        }
        try
        {
            _iRoute.updateRoute(route);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RouteExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(route);
    }
    // DELETE api/route/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Route>> Delete(int id)
    {
        var route = _iRoute.deleteRoute(id);
        return await Task.FromResult(route);
    }

    private bool RouteExists(int id)
    {
        return _iRoute.checkRoute(id);
    }
}
