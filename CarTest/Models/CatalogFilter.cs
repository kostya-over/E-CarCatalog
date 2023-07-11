namespace CarTest.Models;

public class CatalogFilter
{
    public IEnumerable<Car> carList { get; set; }
    public FilterOptions FilterOptions { get; set; }
    public List<string> BodyTypes = new()
    {
        "Sedan",
        "Hatchback",
        "SUV",
        "Crossover",
        "Coupe",
        "Minivan",
        "Van",
        "Coupe SUV",
        "Sport car"
    };
    
    public List<string> Makes = new()
    {
        "Aiways",
        "Audi",
        "BAICK",
        "Bestune",
        "BMW",
        "BYD",
        "CHANGAN",
        "Chevrolet",
        "Dongfeng",
        "Ford",
        "GAC",
        "HiPhi",
        "Honda",
        "Hyundai",
        "Jaguar",
        "Lexus",
        "Mazda",
        "Mercedes",
        "Polestar",
        "Toyota",
        "Tesla",
        "Volkswagen",
        "Xpeng"
    };

    public List<string> SortOptions = new()
    {
        "Default",
        "PriceDesc",
        "PriceAsc"
    };
}