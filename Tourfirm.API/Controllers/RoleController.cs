using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/role")]
[ApiController]
//Контроллер для работы с апи, данные - Роли
public class RoleController: ControllerBase
{
    private readonly IRole _IRole;
    
    public RoleController(IRole iRole)
    {
        _IRole = iRole;
    }
    
    //get api/role
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> Get()
    {
        return await Task.FromResult(await _IRole.getRoles()); 
    }
    
    //get api/role/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> Get(int id)
    {
        var role = await Task.FromResult(await _IRole.getRole(id));
        if (role == null)
            return NotFound();
        return role; 
    }
    
    //post api/role
    [HttpPost]
    public async Task<ActionResult<Role>> Post(Role role)
    {
        await _IRole.addRole(role);
        return await Task.FromResult(role);
    }
    
    // PUT api/role/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Role>> Put(int id, Role role)
    {
        if (id != role.Id)
        {
            return BadRequest();
        }
        try
        {
            _IRole.updateRole(role);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoleExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(role);
    }
    // DELETE api/role/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Role>> Delete(int id)
    {
        var role = _IRole.deleteRole(id);
        return await Task.FromResult(role);
    }
    
    private bool RoleExists(int id)
    {
        return _IRole.checkRole(id);
    }
    
    
}
