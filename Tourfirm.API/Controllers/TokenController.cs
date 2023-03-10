using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameStop.Models.Safety;
using Microsoft.Extensions.Configuration;
using Tourfirm.DAL;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Route("api/token")]
[ApiController]
//Контроллер для работы с апи, данные - Токены
public class TokenController : ControllerBase
{
    
    public IConfiguration _configuration;
    private readonly ApplicationContext _db;

    public TokenController(IConfiguration config, ApplicationContext db)
    {
        _configuration = config;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Account account)
    {
        if (account != null && account.Email != null && account.Login != null && account.Password != null)
        {
            var accountObj = await GetAccount(account.Email, PasswordHasher.HashPassword(account.Password));

            if (accountObj != null)
            {
                //create claims details based on the accountObj information
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("AccountId", accountObj.Id.ToString()),
                    new Claim("Email", accountObj.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return BadRequest("Bad Credentials");
        }
        return BadRequest();
    }

    private async Task<Account> GetAccount(string email, string password)
        {
            return await _db.Account.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
        
}