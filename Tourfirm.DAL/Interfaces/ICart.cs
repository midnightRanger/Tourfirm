using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;
//интерфейс для CRUD корзины
public interface ICart
{
    public Task addCart(Cart cart);
    public void updateCart(Cart cart);
    public Cart deleteCart(in int id);
    
    public bool checkCart(int id);

    public Task<List<Cart>> getCarts();
    public Task<Cart> getCart(int id);
    
    public IQueryable<Cart> getAll(); 
}