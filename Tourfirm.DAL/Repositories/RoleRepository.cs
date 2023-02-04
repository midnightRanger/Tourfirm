using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class RoleRepository : IRole
{
    private readonly ApplicationContext _db;
    
    public RoleRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addRole(Role role)
    {
        _db.Role.Add(role);
        await _db.SaveChangesAsync();
    }

    public void updateRole(Role role)
    {
        _db.Entry(role).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Role deleteRole(in int id)
    {
        Role? role = _db.Role.Find(id);

        if (role != null)
        {
            _db.Role.Remove(role);
            _db.SaveChanges();
            return role;
        }

        throw new ArgumentNullException();
    }

    public bool checkRole(int id)
    {
        return _db.Role.Any(r => r.Id == id);
    }

    public async Task<List<Role>> getRoles()
    {
        return await _db.Role.ToListAsync();
    }

    public async Task<Role> getRole(int id)
    {
        Role? role = await _db.Role.FindAsync(id);

        if (role != null)
        {
            return role;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<Role> getAll()
    {
        return _db.Role; 
    }
}