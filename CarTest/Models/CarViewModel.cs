using System.ComponentModel.DataAnnotations;
using CarTest.Attributes;

namespace CarTest.Models;

public class CarViewModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Brand { get; set; }
    [Required]
    [StringLength(100)]
    public string Model { get; set; }
    [Required]
    [Range(1900, 9999)]
    public int Year { get; set; }
    [Required]
    public double Range { get; set; }
    [Required]
    [Range(0, 200000)]
    public int Price { get; set; }
    [Required]
    public double BatteryCapacity { get; set; }
    [Required]
    public int MotorPower { get; set; }
    [Required]
    [Range(0, 500)]
    public int TopSpeed { get; set; }
    [Required]
    public double Acceleration { get; set; }
    [Required]
    public int ChargingTime { get; set; }
    [Required]
    public string BodyType { get; set; }
    [Required]
    [ValidateImageFile]
    public IEnumerable<IFormFile> Photos { get; set; }
}