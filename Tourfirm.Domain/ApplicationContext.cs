using Microsoft.EntityFrameworkCore;
using Tourfirm.Domain.Entity;

namespace Tourfirm.DAL;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureDeleted(); 
        Database.EnsureCreated();
    }

    public ApplicationContext()
    { 
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //
        // modelBuilder.Entity<Role>().HasData(new[]
        // {
        //     new Role() { Id = 1, Name = "USER", Description="Average User"}
        // });
        //
        // modelBuilder.Entity<Account>().HasData(new[]
        // {
        //     new Account()
        //     {
        //         Id=1, Email = "somemail@mail.ru", Login = "kuznetsov123", Password = "123", isActive = true
        //         
        //     }
        // });
        //
        // modelBuilder.Entity<AccountModel>().HasData(new[]
        // {
        //     new Ac
        // });

        // modelBuilder.Entity<User>().HasData(new[]
        // {
        //     new User() { Id = 1, AccountId = 1, Age = 19, Balance = 0.00, Name = "semen", Surname = "kuznetsov"}
        // });

        
        modelBuilder.Entity<BookingType>().HasData(new[]
        {
            new BookingType() { Id = 1, Name="Garanteed", Description = "Typical garanteed booking"},
            new BookingType() { Id=2, Name="Non-garanteed", Description = "Non-garanteed booking"},
            new BookingType() { Id=3, Name="Super garanteed", Description = "Super Garanteed booking"}
        });
        
            modelBuilder.Entity<Country>().HasData(new[]
            {
                new Country() {Id = 1, Name = "Russia", Climate = "Mild", Language = "Russian", Tours = null, MidTemp = 18},
                new Country() {Id = 2, Name = "Brazil", Climate = "Tropical", Language = "Portugal", Tours = null, MidTemp = 26},
                new Country() {Id = 3, Name = "Israel", Climate = "Subtropical", Language = "Hebrew", Tours = null, MidTemp = 30}
            });
            
            modelBuilder.Entity<Route>().HasData(new[]
            {
                new Route() {Id = 1, Hours = 4, StartPos = "Moscow", EndPost = "Sochi", Type = "Train", Tours = null},
                new Route() {Id = 2, Hours = 26, StartPos = "Moscow", EndPost = "Brasilia", Type = "Plane", Tours = null},
                new Route() {Id = 3, Hours = 15, StartPos = "Moscow", EndPost = "Jerusalem", Type = "Plane", Tours = null},
                
            });
            
            modelBuilder.Entity<HotelProperties>().HasData(new[]
            {
                new HotelProperties() {Id = 1, BookingTypeId = 1, Capacity = 300, Classification = "Hotel", Food = "BB", Stars = 5, Style = "Modern"},
                new HotelProperties() {Id = 2, BookingTypeId = 1, Capacity = 200, Classification = "Motel", Food = "AI", Stars =4, Style = "AR-Deco"},
                new HotelProperties() {Id = 3, BookingTypeId = 1, Capacity = 500, Classification = "Hotel", Food = "AI", Stars =5, Style = "Retro"},

            });

            modelBuilder.Entity<HotelService>().HasData(new[]
            {
                new HotelService() {Id=1, Cost = 300, Name = "Taxi calling", HotelPropertiesId = 1},
                new HotelService() {Id = 2, Cost = 500, Name = "Auto sharing", HotelPropertiesId = 1},
                new HotelService() {Id = 3, Cost = 3200, Name = "Cloth Cleaning", HotelPropertiesId = 2}, 
                new HotelService() {Id = 4, Cost = 120, Name = "Flowers delivery", HotelPropertiesId = 3}
            });
            
            
            modelBuilder.Entity<Hotel>().HasData(new[]
            {
               new Hotel() { Id = 1, Name = "Volga", Rate = 3.3, HotelPropertiesId = 1}, 
               new Hotel() {Id=2, Name = "El Sapacho", Rate = 4.4, HotelPropertiesId = 2},
               new Hotel() {Id=3, Name="HeavyHeaven", Rate = 2.0, HotelPropertiesId = 3}
            });
            
            modelBuilder.Entity<TourType>().HasData(new[]
            {
                new TourType() { Id = 1, Name = "Oversea"}, 
                new TourType() {Id=2, Name = "Internally"} 
            });

            

            modelBuilder.Entity<TourImage>().HasData(new[]
            {
                new TourImage() { Id = 1, Path = "~/images/FRWL1.jpg", TourId = 1 },
                new TourImage() { Id = 2, Path = "~/images/BS1.jpg", TourId = 2 },
                new TourImage() { Id = 3, Path = "~/images/DAS1.jpg", TourId = 3 },
            });
            
            modelBuilder.Entity<Tour>().HasData(new[]
            {
                new Tour() { Id = 1, Name = "From Russia with love", Description = "Average tour on Russian", Cost = 30000, RouteId=1, HotelId = 1, TourTypeId = 2, CountryId = 1},
                new Tour() { Id = 2, Name = "Brazilian Sun", Description = "Let's have a fun in the hottest Country!", Cost = 130000, RouteId=2, HotelId = 2, TourTypeId = 1, CountryId = 2},
                new Tour() { Id = 3, Name = "Deserts and Skorpions", Description = "Put your hat on your head - its really hot there", Cost = 90000, RouteId=3, HotelId = 3, TourTypeId = 2, CountryId = 3}

            });
            
            // modelBuilder.Entity<Review>().HasData(new[]
            // {
            //     new Review() { Id = 1, Text = "Отличный тур! Все понравилось", UserId = 1, IsAccept = true, TourId = 2 },
            //     new Review() { Id = 2, Text = "Были огрехи, но в целом все хорошо", UserId = 1, IsAccept = true, TourId = 2 },
            // });



            base.OnModelCreating(modelBuilder);
    }

    public DbSet<Role> Role { get; set; } = null!;
    public DbSet<Account> Account { get; set; } = null!;
    public DbSet<Cart> Cart { get; set; } = null!;
    public DbSet<Country> Country { get; set; } = null!;
    public DbSet<Hotel> Hotel { get; set; } = null!;
    public DbSet<HotelProperties> HotelProperties { get; set; } = null!;
    public DbSet<HotelService> HotelService { get; set; } = null!;
    public DbSet<PaymentInfo> PaymentInfo { get; set; } = null!;
    public DbSet<Review> Review { get; set; } = null!;
    public DbSet<Route> Route { get; set; } = null!;
    public DbSet<Tour> Tour { get; set; } = null!;
    public DbSet<TourImage> TourImage { get; set; } = null!;
    public DbSet<TourType> TourType { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;
    public DbSet<BookingType> BookingType { get; set; } = null!; 
}