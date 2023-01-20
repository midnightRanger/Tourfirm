using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tourfirm.Controllers;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.UnitTests.ViewTests;

[TestClass]
public class RouteControllerTest
{
    private readonly ILogger<RouteController> _logger;
    [TestMethod]
    public async Task TestRouteIndexView()
    {
        // Arrange
        var routes = new List<Route>()
        {
            new Route() { Id = 1, EndPost = "Osaka", StartPos = "Moscow", Type = "Train" },
            new Route() { Id = 2, EndPost = "Osaka", StartPos = "Moscow2", Type = "Train" },
        };
        
        var routeRepository = new Mock<IRoute>();
        var routeService = new Mock<IRouteService>();
        routeRepository.Setup(e => e.getAll()).Returns(routes.AsQueryable);

        var controller = new RouteController( _logger,  routeRepository.Object, routeService.Object);
        var result =  await controller.RouteIndex("testing") as ViewResult;
        var model = result.Model as IEnumerable<Route>; 
        Assert.AreEqual("RouteIndex", result.ViewName);
        Assert.IsNotNull(model);
    }
}