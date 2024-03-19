using Microsoft.EntityFrameworkCore;

namespace DataService.Models;

public class UsersDbContext : DbContext
{
    private readonly string _connectionString = String.Empty;
    public DbSet<Employee> Employee { get; set; }

    public UsersDbContext(IConfiguration config)
    {
        this._connectionString = config.GetConnectionString("UserdbConnection");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(this._connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.EmployeeId);
    }
}