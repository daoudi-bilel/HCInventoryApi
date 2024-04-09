using Microsoft.EntityFrameworkCore;
using ITInventoryManagementAPI.Models;
public class ITInventoryContext : DbContext
{
    public ITInventoryContext(DbContextOptions<ITInventoryContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>()
            .HasOne(d => d.Employee)
            .WithMany()
            .HasForeignKey(d => d.EmployeeId);
    }
}

