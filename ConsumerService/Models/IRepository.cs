namespace ConsumerService.Models;

public interface IRepository
{
    Task<Employee> GetByIdAsync(int id);
    Task<List<Employee>> GetAllAsync();
    Task AddAsync(Employee obj); 
    Task UpdateAsync(Employee obj);
    Task RemoveAsync(int id);
    Task<List<Employee>> FilterEmployeesAsync(string searchKeyword, int pageNumber, int pageSize);
}