using CarTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarTest.Data;

public class CatalogDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarPhoto> CarPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().ToTable("Cars");
        modelBuilder.Entity<CarPhoto>().ToTable("CarPhotos");
        
        modelBuilder.Entity<CarPhoto>()
            .HasOne(p => p.Car)
            .WithMany(c => c.Photos)
            .HasForeignKey(p => p.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}