using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

//Контроллер для работы с апи, данные - Аккаунт
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/account")]
[ApiController]
public class AccountController: ControllerBase
{
    private readonly IAccount _IAccount;

    public AccountController(IAccount IAccount)
    {
        _IAccount = IAccount; 
    }
    
    //Получение элемента
    //get api/account 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> Get()
    {
        return await Task.FromResult(_IAccount.getAccounts()); 
    }
    
    //Получение элементов
    //get api/account/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> Get(int id)
    {
        var accounts = await Task.FromResult(_IAccount.getAccount(id));
        if (accounts == null)
            return NotFound();
        return accounts; 
    }
    
    //добавление нового элемента
    //post api/account
    [HttpPost]
    public async Task<ActionResult<Account>> Post(Account account)
    {
        await _IAccount.addAccount(account);
        return await Task.FromResult(account);
    }
    
    //обновление элемента
    // PUT api/account/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Account>> Put(int id, Account account)
    {
        if (id != account.Id)
        {
            return BadRequest();
        }
        try
        {
            _IAccount.updateAccount(account);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return await Task.FromResult(account);
    }
    //удаление элемента
    // DELETE api/account/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Account>> Delete(int id)
    {
        var account = _IAccount.deleteAccount(id);
        return await Task.FromResult(account);
    }
    //проверка существует ли
    private bool AccountExists(int id)
    {
        return _IAccount.checkAccount(id);
    }
}
