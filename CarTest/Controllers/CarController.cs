using CarTest.Models;
using CarTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarTest.Controllers;

public class CarController: Controller
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    public async Task<IActionResult> Catalog(FilterOptions options)
    {
        (IEnumerable<Car>, FilterOptions) filter =
            await _carService.GetCars(options);
        return View(new CatalogFilter
        {
            carList = filter.Item1,
            FilterOptions = filter.Item2
        });
    }

    public async Task<IActionResult> CarInfo(int id)
    {
        return View(await _carService.GetCarById(id));
    }
    
    
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AdminPanel()
    {
        return View(await _carService.GetCarsForAdmin());
    }

    public IActionResult AddCar()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCar(CarViewModel carModel)
    {
        if (ModelState.IsValid)
        {
            await _carService.AddCar(carModel);
            return RedirectToAction(nameof(AdminPanel));
        }

        return View(carModel);
    }

    public async Task<IActionResult> EditCar(int id)
    {
        return View(await _carService.GetCarById(id));
    }

    [HttpPost]
    public async Task<IActionResult> EditCar(int id, CarViewModel carModel)
    {
        Car car = await _carService.GetCarById(id);
        if(ModelState.IsValid)
        {
            await _carService.UpdateCar(id, carModel, car);
            return RedirectToAction(nameof(AdminPanel));
        }

        return View(car);
    }
    
    public async Task<IActionResult> DeleteCar(int id)
    {
        await _carService.DeleteCar(id);
        return RedirectToAction(nameof(AdminPanel));
    }
}