using System.Security.Claims;
using GameStop.Response;
using Tourfirm.DAL.ViewModels;

namespace Tourfirm.Service.Implementations;

public interface IAuthService
{
    Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

    Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    // Task<BaseResponse<bool>> ChangeAccount(UserUpdateView model);

}