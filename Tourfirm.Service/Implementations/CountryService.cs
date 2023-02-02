using Microsoft.Extensions.Logging;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;

public class CountryService: ICountryService
{
    private readonly ICountry _countryRepository;
    private readonly ILogger<CountryService> _logger;
    private readonly ApplicationContext _db; 

    public CountryService(ICountry countryRepository, ILogger<CountryService> logger, ApplicationContext db)
    {
        _countryRepository = countryRepository;
        _logger = logger;
        _db = db;
    }

    public async Task<BaseResponse<bool>> CreateCountry(Country country)
    {
         try
         {
             await _countryRepository.addCountry(country);
        
             return new BaseResponse<bool>()
             {
                 Data = true,
                 StatusCode = StatusCode.OK,
                 Description = "Country creation procedure was successfully completed"
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

    public async Task<BaseResponse<bool>> UpdateCountry(Country country)
    {
        try
        {
            if (country == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TourNotFound,
                    Description = "Country not found"
                };
            }
            
            _countryRepository.updateCountry(country);
            await _db.SaveChangesAsync(); 
            
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Country was updated"
            };
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"[UpdateCountry]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        } 
    }

    public Task<BaseResponse<bool>> DeleteCountry(Country country)
    {
        throw new NotImplementedException();
    }
}