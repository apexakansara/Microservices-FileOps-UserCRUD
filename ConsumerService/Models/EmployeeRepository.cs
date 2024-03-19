using Microsoft.EntityFrameworkCore;

namespace ConsumerService.Models;

public class EmployeeRepository : IRepository
{
    private UsersDbContext _dbContext;

    public EmployeeRepository(UsersDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        return await this._dbContext.Employee.FindAsync(id);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await this._dbContext.Employee.ToListAsync();
    }

    public async Task AddAsync(Employee employee)
    {
        this._dbContext.Employee.Add(employee);
        await this._dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        this._dbContext.Entry(employee).State = EntityState.Modified;
        await this._dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        Employee employee = await GetByIdAsync(id);
        if (employee!=null)
        {
            this._dbContext.Remove(employee);
            await this._dbContext.SaveChangesAsync();
        }
    }
}