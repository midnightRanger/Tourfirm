using System.Security.Claims;
using GameStop;
using GameStop.Models.Safety;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tourfirm.DAL.Interfaces;
using Tourfirm.DAL.ViewModels;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;

namespace Tourfirm.Service.Implementations;
//Сервис для взаимодействия с аккаунтами
public class AuthService: IAuthService
{
    
    private readonly IAccount _accountRepository;
    private readonly IUser _userRepository;
    private readonly ICart _cartRepository;
    private readonly IRole _roleRepository;
     private readonly ILogger<AuthService> _logger; 
    
    public AuthService(IAccount accountRepository,
        ILogger<AuthService> logger, IUser userRepository, ICart cartRepository, IRole roleRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _logger = logger;
        _cartRepository = cartRepository;
        _roleRepository = roleRepository;

        //TODO
        //_proFileRepository = proFileRepository;
    }

    public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
    {
         try
        {
            var user = await _accountRepository.getAll().FirstOrDefaultAsync(x => x.Login == model.Login);
            if (user != null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Пользователь с таким логином уже есть",
                };
            }

            List<Role> roles = new List<Role>();
            Role? role = await _roleRepository.getRole(1); 
            
            user = new Account()
            {
                Login = model.Login,
                Email = model.Email,
                isActive = true,
                Password = PasswordHasher.HashPassword(model.Password),
            };
            user.Roles.Add(role);

            var userInfo = new User()
            {
                Name = model.Name,
                Surname = model.Surname,
                AccountId = user.Id,
                Account = user,
                Age = model.Age,
                Balance = 0.00
            };

            var userCart = new Cart()
            {
                UserId= userInfo.Id, 
                User = userInfo,
                Sum = 0.00, 
                Value = 0,
                Tours = null
            };



            await _accountRepository.addAccount(user);
            await _userRepository.addUser(userInfo);
            await _cartRepository.addCart(userCart);
            var result = Authenticate(user);

            return new BaseResponse<ClaimsIdentity>()
            {
                Data = result,
                Description = "Объект добавился",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Register]: {ex.Message}");
            return new BaseResponse<ClaimsIdentity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
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

            if (user.isActive == false)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Sorry, your account was banned:("
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
        var accountRole = _accountRepository.getAll().Include(r => r.Roles).FirstOrDefault(a=>a.Id == account.Id);
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login)
        };

        foreach (var role in accountRole.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }
        
        return new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
}