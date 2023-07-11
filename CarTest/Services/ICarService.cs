using CarTest.Models;

namespace CarTest.Services;

public interface ICarService
{
    // Task<(IPagedList<Car>, FilterOptions)> GetCars(string[] bodyType, int priceMin, int priceMax, string[] make, string model,
    //     string sort, int pageNumber, int pageSize);
    Task<(IEnumerable<Car>, FilterOptions)> GetCars(FilterOptions options);
    Task<Car> GetCarById(int id);
    Task<IEnumerable<Car>> GetCarsForAdmin();
    Task AddCar(CarViewModel carModel);
    Task UpdateCar(int id, CarViewModel carModel, Car car);
    Task DeleteCar(int id);
}