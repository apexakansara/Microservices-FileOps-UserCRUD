using DataService.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private UserHelper _userHelper;

    public UserController(UserHelper helper)
    {
        this._userHelper = helper;
    }

    [HttpGet]
    [ApiVersion("1.0")]
    public async Task<IActionResult> GetEmployees([FromQuery] string? searchKeyword,
        int pageNumber=1,
        int pageSize=10)
    {

        List<Employee> employees = await this._userHelper.GetFilteredEmployeesAsync(searchKeyword,
            pageNumber, 
            pageSize);

        return Ok(employees);
    }
}