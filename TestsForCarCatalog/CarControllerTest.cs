using CarTest.Controllers;
using CarTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using NuGet.ContentModel;

namespace TestsForCarCatalog;

public class CarControllerTest
{
    private readonly CarController _controller;

    public CarControllerTest()
    {
        Mock<ICarService> mockCarService = new();
        _controller = new CarController(mockCarService.Object);
    }
    [Fact]
    public async Task Catalog_ReturnsViewResult()
    {
        //Arrange
        
        var filterOptions = new CarTest.Models.FilterOptions
        {
            PriceMin = 10000,
            PriceMax = 50000,
            BodyType = new List<string> { "Sedan", "SUV" },
            Make = new List<string> { "Toyota", "Honda" },
            Model = "Camry",
            SortOrder = "Descending",
            Sort = "Price"
        };
        //Act
        var result = await _controller.Catalog(filterOptions);
        
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }

    [Fact]
    public async Task AdminPanel_WithValidUser_ReturnsViewResult()
    {
        // Arrange

        // Act
        var result = await _controller.AdminPanel();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }

    [Fact]
    public async Task AddCar_WithValidCarViewModel_ReturnsViewResult()
    {
        // Arrange
        var carViewModel = new CarTest.Models.CarViewModel
        {
            Brand = "Tesla",
            Model = "Model 3",
            Year = 2022,
            Range = 300,
            Price = 50000,
            BodyType = "Sedan",
            BatteryCapacity = 75,
            MotorPower = 250,
            TopSpeed = 140,
            Acceleration = 5.6,
            ChargingTime = 8,
            Photos = new List<IFormFile>()
        };
        
        //Act
        var result = await _controller.AddCar(carViewModel);
        
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(_controller.AdminPanel), viewResult.ActionName);
    }
    
    [Fact]
    public async Task CarInfo_WithValidId_ReturnsViewResult()
    {
        //Arrange
        var carId = 1;
        
        //Act
        var result = await _controller.CarInfo(carId);
        
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }

    [Fact]
    public async Task EditCar_WithValidId_ReturnsViewResult()
    {
        //Arrange
        var carId = 1;
        
        //Act
        var result = await _controller.EditCar(carId);
        
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }

    [Fact]
    public async Task EditCar_WithValidIdAndCarViewModel_ReturnsViewResult()
    {
        //Arrange
        var carId = 1;
        var carViewModel = new CarTest.Models.CarViewModel
        {
            Brand = "Tesla",
            Model = "Model 3",
            Year = 2022,
            Range = 300,
            Price = 50000,
            BodyType = "Sedan",
            BatteryCapacity = 75,
            MotorPower = 250,
            TopSpeed = 140,
            Acceleration = 5.6,
            ChargingTime = 8,
            Photos = new List<IFormFile>()
        };
        
        //Act
        var result = await _controller.EditCar(carId, carViewModel);
        
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(_controller.AdminPanel), viewResult.ActionName);
    }

    [Fact]
    public async Task DeleteCar_WithValidId_ReturnsViewResult()
    {
        //Assert 
        var carId = 1;
        
        //Act
        var result = await _controller.DeleteCar(carId);
        
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(_controller.AdminPanel), viewResult.ActionName);
    }
}