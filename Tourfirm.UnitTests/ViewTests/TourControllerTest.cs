using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore; 
using Moq;
using Tourfirm.Controllers;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tourfirm.UnitTests.ViewTests;

[TestClass]
public class TourControllerTest
{

    [TestMethod]
    public async Task TestTourIndexView()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour
            {
                Id = 1, Carts = null, Cost = 300,
                Country = new Country() { Name = "Russia", Climate = "Cold", Id = 1, MidTemp = 30, Language = "Riss" },
                Description = "Test", Hotel = new Hotel()
                {
                    HotelProperties = new HotelProperties()
                    {
                        BookingType = new BookingType() { Description = "ss", Id = 1, Name = "sa" }, Capacity = 100,
                        BookingTypeId = 2,
                        Classification = "hsa", Food = "BA", HotelServices = new List<HotelService>()
                        {
                            new HotelService() { Cost = 100, Name = "213", HotelPropertiesId = 1 }
                        },
                        Id = 2, Stars = 5, Style = "Modern"
                    }
                },
                Name = "1",
                Reviews = new List<Review>()
                {
                    new Review()
                    {
                        Id = 1, IsAccept = true, IsRecommend = true, Text = "ses", TourId = 1, User = new User()
                        {
                            Account = new Account()
                            {
                                Email = "@3012@", Id = 1, isActive = true, Login = "Sadas", Password = "sakda",
                                Roles = null
                            },
                            AccountId = 1, Age = 19, Balance = 200, Cart = null, Id = 2, Name = "sa", Surname = "sakl"

                        }
                    }
                },
                CountryId = 1, HotelId = 1
            },
            new Tour
            {
                Id = 2, Carts = null, Cost = 300, Country = null, Description = "Test", Hotel = null, Name = "1",
                Reviews = null,
                CountryId = 1, HotelId = 1
            }
        };
        
        var _tourRepository = new Mock<ITour>();
        _tourRepository.Setup(e => e.getAll()).Returns(tours.AsQueryable);
         
        var controller = new TourController(_tourRepository.Object, );
        var result =  await controller.TourIndex("testing") as ViewResult;
        var model = result.Model as IEnumerable<Tour>; 
        Assert.AreEqual("TourIndex", result.ViewName);
        Assert.IsNotNull(model);
    }
}