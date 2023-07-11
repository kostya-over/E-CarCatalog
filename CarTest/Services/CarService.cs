using CarTest.Data;
using CarTest.Models;
using Microsoft.EntityFrameworkCore;


namespace CarTest.Services;

public class CarService : ICarService
{
    private readonly CatalogDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CarService> _logger;

    public CarService(CatalogDbContext context, 
        IWebHostEnvironment environment, ILogger<CarService> logger)
    {
        _context = context;
        _environment = environment;
        _logger = logger;
    }

    public async Task<(IEnumerable<Car>, FilterOptions)> GetCars(FilterOptions options)
    {
        var cars = from c in _context.Cars select c;
        
        if (options.BodyType.Any())
            cars = cars.Where(c => options.BodyType.Contains(c.BodyType));

        if (options.Make.Any())
            cars = cars.Where(c => options.Make.Contains(c.Brand));

        if (!string.IsNullOrEmpty(options.Model))
            cars = cars.Where(c => c.Model.Contains(options.Model));

        if (options.PriceMin > 0 && options.PriceMax > 0)
            cars = cars.Where(c => c.Price >= options.PriceMin && c.Price <= options.PriceMax);
        else
        {
            options.PriceMin = 1;
            options.PriceMax = 500_000;
        }
        switch (options.Sort)
        {
            case "PriceAsc":
                cars = cars.OrderBy(c => c.Price);
                break;
            case "PriceDesc":
                cars = cars.OrderByDescending(c => c.Price);
                break;
            default:
                cars = cars.OrderByDescending(c => c.Id);
                break;
        }

        cars = cars.Include(c => c.Photos);
        options.SortOrder = options.Sort;
       return (await cars.ToListAsync(), options);
    }

    public async Task<Car> GetCarById(int id)
    {
        if(!CarExists(id))
            throw new ArgumentException($"Car with Id: {id} does not exists.");
        
        return await _context.Cars.Include(c => c.Photos).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Car>> GetCarsForAdmin()
    {
        return await _context.Cars.ToListAsync();
    }

    public async Task AddCar(CarViewModel carModel)
    {
        Car car = new Car();
        MapCarModel(ref car, carModel);

        await AddPhotos(car, carModel.Photos);
            
        _context.Add(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCar(int id, CarViewModel carModel, Car car)
    {
        if (id != carModel.Id)
        {
            throw new ArgumentException("Id parameter does not match the carModel.Id property.");
        }

        MapCarModel(ref car, carModel);

        await DeletePhotos(car.Photos);

        await AddPhotos(car, carModel.Photos);

        _context.Update(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCar(int id)
    {
        Car car = await GetCarById(id);
        
        await DeletePhotos(car.Photos);

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }
    
    private async Task DeletePhotos(IEnumerable<CarPhoto> photos)
    {
        foreach (var photo in photos)
        {
            string fileLocation = Path.Combine(_environment.WebRootPath, "images", photo.Uri);
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
                _logger.LogInformation("File {CarPhoto} was deleted", photo);
            }
            else
            {
                _logger.LogWarning("File {CarPhoto} not found", photo);
            }
        }

        _context.CarPhotos.RemoveRange(photos);
    }

    private async Task AddPhotos(Car car, IEnumerable<IFormFile> photos)
    {
        string imageDir = Path.Combine(_environment.WebRootPath, "images");
        if (!Directory.Exists(imageDir))
        {
            Directory.CreateDirectory(imageDir);
        }

        if (!photos.Any())
        {
            CarPhoto carPhoto = new CarPhoto
            {
                Uri = "~/images/noContent.png",
                CarId = car.Id
            };
                
            car.Photos.Add(carPhoto);
            return;
        }

        foreach (var photo in photos)
        {
            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                
                CarPhoto carPhoto = new CarPhoto
                {
                    Uri = fileName,
                    CarId = car.Id
                };
                
                car.Photos.Add(carPhoto);
            }
        }
    }
    
    private bool CarExists(int id)
    {
        return _context.Cars.Any(c => c.Id == id);
    }

    private void MapCarModel(ref Car car, CarViewModel carModel)
    {
        car.Brand = carModel.Brand;
        car.Model = carModel.Model;
        car.Year = carModel.Year;
        car.Range = carModel.Range;
        car.Price = carModel.Price;
        car.BatteryCapacity = carModel.BatteryCapacity;
        car.MotorPower = carModel.MotorPower;
        car.TopSpeed = carModel.TopSpeed;
        car.Acceleration = carModel.Acceleration;
        car.ChargingTime = carModel.ChargingTime;
        car.BodyType = carModel.BodyType;
    }
}