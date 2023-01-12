using System.Security.Claims;
using Tourfirm.DAL.ViewModels;
using Tourfirm.Domain.Response;

namespace Tourfirm.Service.Implementations;

public interface IAuthService
{
    Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

    Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    // Task<BaseResponse<bool>> ChangeAccount(UserUpdateView model);

}