using Microsoft.EntityFrameworkCore;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Account> Account { get; set; } = null!;
    public DbSet<Cart> Cart { get; set; } = null!;
    public DbSet<Country> Country { get; set; } = null!;
    public DbSet<Hotel> Hotel { get; set; } = null!;
    public DbSet<HotelProperties> HotelProperties { get; set; } = null!;
    public DbSet<HotelService> HotelService { get; set; } = null!;
    public DbSet<PaymentInfo> PaymentInfo { get; set; } = null!;
    public DbSet<Review> Review { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;
    public DbSet<Route> Route { get; set; } = null!;
    public DbSet<Tour> Tour { get; set; } = null!;
    public DbSet<TourImage> TourImage { get; set; } = null!;
    public DbSet<TourType> TourType { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;
}