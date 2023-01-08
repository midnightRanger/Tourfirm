using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;

public class CartRepository : ICart
{
    private ApplicationContext _db = new();

    public CartRepository(ApplicationContext db)
    {
        _db = db; 
    }
    
    public async Task addCart(Cart cart)
    {
        _db.Cart.Add(cart);
        await _db.SaveChangesAsync();
    }

    public void updateCart(Cart cart)
    {
        _db.Entry(cart).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Cart deleteCart(in int id)
    {
        Cart? cart = _db.Cart.Find(id);

        if (cart != null)
        {
            _db.Cart.Remove(cart);
            _db.SaveChanges();
            return cart;
        }

        throw new ArgumentNullException();
    }

    public bool checkCart(int id)
    {
        return _db.Cart.Any(c => c.Id == id);
    }

    public async Task<List<Cart>> getCarts()
    {
        return await _db.Cart.ToListAsync();
    }

    public async Task<Cart> getCart(int id)
    {
        Cart? cart = await _db.Cart.FindAsync(id);

        if (cart != null)
        {
            return cart;
        }

        throw new ArgumentNullException();
    }

    public IQueryable<Cart> getAll()
    {
        return _db.Cart; 
    }
}