using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;

public interface IPaymentInfo
{
    public Task addPaymentInfo(PaymentInfo paymentInfo);
    public void updatePaymentInfo(PaymentInfo paymentInfo);
    public PaymentInfo deletePaymentInfo(in int id);
    
    public bool checkPaymentInfo(int id);

    public Task<List<PaymentInfo>> getPaymentInfos();
    public Task<PaymentInfo> getPaymentInfo(int id);
    
    public IQueryable<PaymentInfo> getAll(); 
}