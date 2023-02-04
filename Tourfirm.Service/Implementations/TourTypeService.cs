using Microsoft.Extensions.Logging;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;
//Сервис с функциями для типов туров 
public class TourTypeService : ITourTypeService
{
    private readonly ITourType _tourTypeRepository;
    private readonly ILogger<TourTypeService> _logger;
    private readonly ApplicationContext _db;

    public TourTypeService(ITourType tourTypeRepository, ILogger<TourTypeService> logger, ApplicationContext db)
    {
        _tourTypeRepository = tourTypeRepository;
        _logger = logger;
        _db = db;
    }

    public async Task<BaseResponse<bool>> CreateTourType(TourType tourType)
    {
        try
        {
            await _tourTypeRepository.addTourType(tourType);

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Tour type creation procedure was successfully completed"
            };
        }

        catch(Exception ex)
        {
            _logger.LogError(ex, $"[Creating Tour Type Procedure]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<bool>> UpdateTourType(TourType tourType)
    {
        try
        {
            if (tourType == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TourNotFound,
                    Description = "Tour Type not found"
                };
            }
            
            _tourTypeRepository.updateTourType(tourType);
            await _db.SaveChangesAsync(); 
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Tour type was updated"
            };
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"[UpdateTourType]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        } 
    }

    public async Task<BaseResponse<bool>> DeleteTourType(TourType tourType)
    {
        try
        {
            _tourTypeRepository.deleteTourType(tourType.Id);
             
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Tour type delete procedure was successfully completed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Deleting Tour Type Procedure]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}