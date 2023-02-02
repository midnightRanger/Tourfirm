using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;

namespace Tourfirm.Service.Interfaces;

public interface ICountryService
{
    Task<BaseResponse<bool>> CreateCountry(Country country);
    Task<BaseResponse<bool>> UpdateCountry(Country country);
    Task<BaseResponse<bool>> DeleteCountry(Country country);
}