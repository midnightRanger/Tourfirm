using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class ChequeRepository: ICheque
{
    private ApplicationContext _db = new();

    public ChequeRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addCheque(Cheque cheque)
    {
        _db.Cheque.Add(cheque);
        await _db.SaveChangesAsync();
    }

    public void updateCheque(Cheque cheque)
    {
        _db.Entry(cheque).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Cheque deleteCheque(in int id)
    {
        Cheque? cheque = _db.Cheque.Find(id);

        if (cheque != null)
        {
            _db.Cheque.Remove(cheque);
            _db.SaveChanges();
            return cheque;
        }

        throw new ArgumentNullException();
    }

    public bool checkCheque(int id)
    {
        return _db.Cheque.Any(c => c.Id == id);
    }

    public async Task<List<Cheque>> getCheques()
    {
        return await _db.Cheque.ToListAsync();
    }

    public async Task<Cheque> getCheque(int id)
    {
        Cheque? cheque = await _db.Cheque.FindAsync(id);

        if (cheque != null)
        {
            return cheque;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<Cheque> getAll()
    {
        return _db.Cheque; 
    }
}