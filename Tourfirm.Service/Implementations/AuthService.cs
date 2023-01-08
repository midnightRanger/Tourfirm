using System.Security.Claims;
using GameStop;
using GameStop.Models.Safety;
using GameStop.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tourfirm.DAL.Interfaces;
using Tourfirm.DAL.ViewModels;
using Tourfirm.Domain.Entity;

namespace Tourfirm.Service.Implementations;

public class AuthService: IAuthService
{
    
    private readonly IAccount _accountRepository;
    private readonly IUser _userRepository;
    private readonly ICart _cartRepository; 
    private readonly ILogger<AuthService> _logger; 
    
    public AuthService(IAccount accountRepository,
        ILogger<AuthService> logger, IUser userRepository, ICart cartRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _logger = logger;
        _cartRepository = cartRepository;

        //TODO
        //_proFileRepository = proFileRepository;
    }
    
    public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
    {
        try
        {
            var user = await _accountRepository.getAll().FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Пользователь не найден"
                };
            }

            if (user.Password != PasswordHasher.HashPassword(model.Password))
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Неверный пароль или логин"
                };
            }
            var result = Authenticate(user);

            return new BaseResponse<ClaimsIdentity>()
            {
                Data = result,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Login]: {ex.Message}");
            return new BaseResponse<ClaimsIdentity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    
    
    private ClaimsIdentity Authenticate(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Roles[0].Name)
        };
        return new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
}