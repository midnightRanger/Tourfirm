using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Repositories;
//репозиторий, имплементирующий интерфейс
public class ReviewRepository : IReview
{
    private readonly ApplicationContext _db;
    
    public ReviewRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task addReview(Review review)
    {
        _db.Review.Add(review);
        await _db.SaveChangesAsync();
    }

    public void updateReview(Review review)
    {
        _db.Entry(review).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public Review deleteReview(in int id)
    {
        Review? review = _db.Review.Find(id);

        if (review != null)
        {
            _db.Review.Remove(review);
            _db.SaveChanges();
            return review;
        }

        throw new ArgumentNullException();
    }

    public bool checkReview(int id)
    {
        return _db.Review.Any(r => r.Id == id);
    }

    public async Task<List<Review>> getReviews()
    {
        return await _db.Review.ToListAsync();
    }

    public async Task<Review> getReview(int id)
    {
        Review? review = await _db.Review.FindAsync(id);

        if (review != null)
        {
            return review;
        }

        throw new ArgumentNullException();
    }


    public IQueryable<Review> getAll()
    {
        return _db.Review; 
    }
}