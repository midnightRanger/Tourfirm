using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class PaymentInfoRepository : IPaymentInfo
{
    private ApplicationContext _db = new();

    public PaymentInfoRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addPaymentInfo(PaymentInfo paymentInfo)
    {
        _db.PaymentInfo.Add(paymentInfo);
        await _db.SaveChangesAsync();
    }

    public void updatePaymentInfo(PaymentInfo paymentInfo)
    {
        _db.Entry(paymentInfo).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public PaymentInfo deletePaymentInfo(in int id)
    {
        PaymentInfo? paymentInfo = _db.PaymentInfo.Find(id);

        if (paymentInfo != null)
        {
            _db.PaymentInfo.Remove(paymentInfo);
            _db.SaveChanges();
            return paymentInfo;
        }

        throw new ArgumentNullException();
    }

    public bool checkPaymentInfo(int id)
    {
        return _db.PaymentInfo.Any(h => h.Id == id);
    }

    public async Task<List<PaymentInfo>> getPaymentInfos()
    {
        return await _db.PaymentInfo.ToListAsync();
    }

    public async Task<PaymentInfo> getPaymentInfo(int id)
    {
        PaymentInfo? paymentInfo = await _db.PaymentInfo.FindAsync(id);

        if (paymentInfo != null)
        {
            return paymentInfo;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<PaymentInfo> getAll()
    {
        return _db.PaymentInfo; 
    }
}