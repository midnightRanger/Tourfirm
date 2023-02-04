using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;
//интерфейс для CRUD функций с пользователями
public interface IUser
{
    public Task addUser(User user);
    public void updateUser(User user);
    public User deleteUser(in int id);
    
    public bool checkUser(int id);

    public List<User> getUsers();
    public User getUser(int id);
    
    public IQueryable<User> getAll(); 
}