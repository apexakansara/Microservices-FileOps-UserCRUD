namespace DataService.Models;

public interface IRepository
{
    Task<Employee> GetByIdAsync(int id);
    Task<List<Employee>> GetAllAsync();
    Task AddAsync(Employee obj); 
    Task UpdateAsync(Employee obj);
    Task RemoveAsync(int id);
}