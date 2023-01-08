using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/user")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly IUser _IUser;

    public UserController(IUser iUser)
    {
        _IUser = iUser; 
    }
    
    //get api/user 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        return await Task.FromResult(_IUser.getUsers()); 
    }
    
    //get api/user/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var users = await Task.FromResult(_IUser.getUser(id));
        if (users == null)
            return NotFound();
        return users; 
    }
    
    //post api/user
    [HttpPost]
    public async Task<ActionResult<User>> Post(User user)
    {
        _IUser.addUser(user);
        return await Task.FromResult(user);
    }
    
    // PUT api/user/5
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Put(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }
        try
        {
            _IUser.updateUser(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return await Task.FromResult(user);
    }
    // DELETE api/user/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> Delete(int id)
    {
        var user = _IUser.deleteUser(id);
        return await Task.FromResult(user);
    }

    private bool UserExists(int id)
    {
        return _IUser.checkUser(id);
    }
}
