using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс 
public class AccountRepository : IAccount
{
    private ApplicationContext _db = new();

    public AccountRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addAccount(Account account)
    {
        _db.Account.Add(account);
        await _db.SaveChangesAsync();
    }

    public void updateAccount(Account account)
    {
        _db.Entry(account).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Account deleteAccount(in int id)
    {
        Account? account = _db.Account.Find(id);

        if (account != null)
        {
            _db.Account.Remove(account);
            _db.SaveChanges();
            return account;
        }

        throw new ArgumentNullException();
    }

    public bool checkAccount(in int id)
    {
        throw new NotImplementedException();
    }

    public bool checkAccount(int id)
    {
        return _db.Account.Any(e => e.Id == id);
    }

    public List<Account> getAccounts()
    {
        return _db.Account.ToList();
    }

    public Account getAccount(int id)
    {
        Account? account = _db.Account.Find(id);

        if (account != null)
        {
            return account;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<Account> getAll()
    {
        return _db.Account; 
    }
}