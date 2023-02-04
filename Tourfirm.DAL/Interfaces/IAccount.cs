using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

//интерфейс для CRUD функций 
public interface IAccount
{
    public Task addAccount(Account account);
    public void updateAccount(Account account);
    public Account deleteAccount(in int id);
    
    public bool checkAccount(int id);

    public List<Account> getAccounts();
    public Account getAccount(int id);

    public IQueryable<Account> getAll(); 
}