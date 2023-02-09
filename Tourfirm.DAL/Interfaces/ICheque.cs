using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface ICheque
{
    public Task addCheque(Cheque cheque);
    public void updateCheque(Cheque cheque);
    public Cheque deleteCheque(in int id);
    
    public bool checkCheque(int id);

    public Task<List<Cheque>> getCheques();
    public Task<Cheque> getCheque(int id);
    
    public IQueryable<Cheque> getAll(); 
}