using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class UserRepository : IUser
{
    private readonly ApplicationContext _db = new();

    public UserRepository(ApplicationContext db)
    {
        _db = db; 
    }

    public List<User> getUsers()
    {
        return _db.User.ToList();
    }

    public User getUser(int id)
    {
        User? user = _db.User.Find(id);
        if (user != null)
        {
            return user;
        }
        throw new ArgumentNullException();
    }

    public IQueryable<User> getAll()
    {
        return _db.User; 
    }

    public async Task addUser(User user)
    {
        await _db.User.AddAsync(user);
        await _db.SaveChangesAsync();
    }
    
    public void updateUser(User user)
    {
        _db.Entry(user).State = EntityState.Modified;
        _db.SaveChanges();
    }
    
    public User deleteUser(in int id)
    {
        User? user = _db.User.Find(id);

        if (user != null)
        {
            _db.User.Remove(user);
            _db.SaveChanges();
            return user;
        }

        throw new ArgumentNullException();
    }
    
    public bool checkUser(int id)
    {
        return _db.User.Any(u => u.Id == id);
    }
}