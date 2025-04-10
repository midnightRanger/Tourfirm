using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tourfirm.API.Controllers;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.DAL.Repositories;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Implementations;
using Tourfirm.Service.Interfaces;
using IHotelService = Tourfirm.DAL.Interfaces.IHotelService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
// Add services to the container.



builder.Services.AddTransient<IAccount, AccountRepository>();
builder.Services.AddTransient<IUser, UserRepository>();
builder.Services.AddTransient<ICart, CartRepository>();
builder.Services.AddTransient<ICountry, CountryRepository>();
builder.Services.AddTransient<IHotel, HotelRepository>();
builder.Services.AddTransient<IHotelProperties, HotelPropertiesRepository>();
builder.Services.AddTransient<IHotelService, HotelServiceRepository>();
builder.Services.AddTransient<IPaymentInfo, PaymentInfoRepository>();
builder.Services.AddTransient<IReview, ReviewRepository>();
builder.Services.AddTransient<IRole, RoleRepository>();
builder.Services.AddTransient<IRoute, RouteRepository>();
builder.Services.AddTransient<ITour, TourRepository>();
builder.Services.AddTransient<ITourImage, TourImageRepository>();
builder.Services.AddTransient<ITourType, TourTypeRepository>();
builder.Services.AddTransient<IBookingType, BookingRepository>();
builder.Services.AddTransient<ICheque, ChequeRepository>();
builder.Services.AddTransient<ITourBooking, TourBookingRepository>(); 

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<ITourTypeService, TourTypeService>();
builder.Services.AddScoped<IHotelFuncService, HotelFuncService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITourBookingService, TourBookingService>();

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });  

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();

builder.Services.AddMvc()
    .AddApplicationPart(typeof(TokenController).GetTypeInfo().Assembly);
builder.Services.AddMvc()
    .AddApplicationPart(typeof(UserController).GetTypeInfo().Assembly);

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql($"server={DbConnection.Server};User Id={DbConnection.UserId};" +
                      $"Password={DbConnection.Password};Port={DbConnection.Port};Database={DbConnection.Database};",
        o => o.UseNodaTime());  
    options.LogTo(Console.WriteLine);
    options.EnableSensitiveDataLogging();   
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Auth/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Auth/Login");
        
    });
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var serviceScope = serviceScopeFactory.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM https://www.youtube.com/");
    await next.Invoke();
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();