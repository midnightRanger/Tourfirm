using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL.Interfaces;
//интерфейс для CRUD функций с обзорами
public interface IReview
{
    public Task addReview(Review review);
    public void updateReview(Review review);
    public Review deleteReview(in int id);
    
    public bool checkReview(int id);

    public Task<List<Review>> getReviews();
    public Task<Review> getReview(int id);

    public IQueryable<Review> getAll(); 
}