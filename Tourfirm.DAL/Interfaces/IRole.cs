using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface IRole
{
    public Task addRole(Role role);
    public void updateRole(Role role);
    public Role deleteRole(in int id);
    
    public bool checkRole(int id);

    public Task<List<Role>> getRoles();
    public Task<Role> getRole(int id);
    
    public IQueryable<Role> getAll(); 
}