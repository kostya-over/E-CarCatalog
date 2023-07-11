namespace CarTest.Models;

public class FilterOptions
{
    public int PriceMin { get; set; }
    public int PriceMax { get; set; }
    public IEnumerable<string>? BodyType { get; set; } = new List<string>();
    public IEnumerable<string>? Make { get; set; } = new List<string>();
    public string? Model { get; set; }
    public string? SortOrder { get; set; }
    public string? Sort { get; set; }
}