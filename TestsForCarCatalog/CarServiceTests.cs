using CarTest.Data;
using CarTest.Models;
using CarTest.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestsForCarCatalog;

public class CarServiceTests
{
    private readonly CatalogDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CarService> _logger;
    
    public CarServiceTests()
    {
        // Create an in-memory database for testing
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new CatalogDbContext(options);

        // Create mock objects for dependencies
        _environment = Mock.Of<IWebHostEnvironment>();
        _logger = Mock.Of<ILogger<CarService>>();
    }
    
    [Fact]
    public async Task GetCars_WithFilterOptions_ReturnsFilteredCars()
    {
        // Arrange
        var service = new CarService(_context, _environment, _logger);
        var filterOptions = new FilterOptions
        {
            PriceMin = 10000,
            PriceMax = 50000,
            BodyType = new List<string> { "Sedan", "SUV" },
            Make = new List<string> { "Toyota", "Honda" },
            Model = "Camry",
            SortOrder = "Descending",
            Sort = "Price"
        };

        // Act
        var result = await service.GetCars(filterOptions);

        // Assert
        var cars = Assert.IsAssignableFrom<IEnumerable<Car>>(result.Item1);
        var filteredOptions = Assert.IsType<FilterOptions>(result.Item2);
        Assert.True(cars.All(c => c.Price >= filterOptions.PriceMin && c.Price <= filterOptions.PriceMax));
        Assert.Equal(filterOptions, filteredOptions);
    }

    [Fact]
    public async Task GetCarById_WithValidId_ReturnsCar()
    {
        //Arrange
        var service = new CarService(_context, _environment, _logger);
        var carId = 42;
        
        var carDb = new Car
        {
            Id = carId,
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
            Photos = new List<CarPhoto>()
        };
        _context.Cars.Add(carDb);
        await _context.SaveChangesAsync();
        
        //Act
        var result = await service.GetCarById(carId);
        
        //Assert
        var car = Assert.IsType<Car>(result);
        Assert.Equal(carId, car.Id);
    }

    [Fact]
    public async Task GetCarsForAdmin_ReturnsIEnumerableOfCars()
    {
        //Arrange
        var service = new CarService(_context, _environment, _logger);
        
        //Act
        var result = await service.GetCarsForAdmin();
        
        //Assert
        Assert.IsAssignableFrom<IEnumerable<Car>>(result);
    }
}