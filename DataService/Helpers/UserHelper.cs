using DataService.Models;

public class UserHelper
{
    protected readonly EmployeeRepository _repository;
    public UserHelper(EmployeeRepository repository)
    {
        this._repository = repository;
    }

    public async Task<List<Employee>> GetFilteredEmployeesAsync(string searchKeyword,
        int pageNumber,
        int pageSize)
    {
        return await this._repository.FilterEmployeesAsync(
            searchKeyword, 
            pageNumber, 
            pageSize);
    }
}