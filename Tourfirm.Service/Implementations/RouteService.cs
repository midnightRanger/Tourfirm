using Microsoft.Extensions.Logging;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;
//Сервис с функциями для маршрута
public class RouteService : IRouteService
{
    private readonly ILogger<RouteService> _logger;
    private readonly ApplicationContext _db;
    private readonly IRoute _routeRepository;

    public RouteService(ILogger<RouteService> logger, ApplicationContext db, IRoute routeRepository)
    {
        _logger = logger;
        _db = db;
        _routeRepository = routeRepository;
    }

    public async Task<BaseResponse<bool>> CreateRoute(Route route)
    {
         try
         {
             await _routeRepository.addRoute(route);

             return new BaseResponse<bool>()
             {
                 Data = true,
                 StatusCode = StatusCode.OK,
                 Description = "Route creation procedure was successfully completed"
             };
         }

         catch(Exception ex)
         {
             _logger.LogError(ex, $"[Creating Route Procedure]: {ex.Message}");
             return new BaseResponse<bool>()
             {
                 Description = ex.Message,
                 StatusCode = StatusCode.InternalServerError
             };
         }
    }

    public async Task<BaseResponse<bool>> UpdateRoute(Route routeModel)
    {
        try
        {
            if (routeModel == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TourNotFound,
                    Description = "Route not found"
                };
            }
            
            _routeRepository.updateRoute(routeModel);
            await _db.SaveChangesAsync(); 
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Route was updated"
            };
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"[UpdateRoute]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        } 
    }

    public async Task<BaseResponse<bool>> DeleteRoute(Route route)
    {
        try
        {
            _routeRepository.deleteRoute(route.Id);
             
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Route delete procedure was successfully completed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Deleting Route Procedure]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}