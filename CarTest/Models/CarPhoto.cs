using System.ComponentModel.DataAnnotations;

namespace CarTest.Models;

public class CarPhoto
{
    [Key]
    public int Id { get; set; }
    public string Uri { get; set; } = null!;
    public int CarId { get; set; }
    public Car Car { get; set; }
}