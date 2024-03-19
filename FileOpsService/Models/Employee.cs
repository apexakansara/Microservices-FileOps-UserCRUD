using System.ComponentModel.DataAnnotations;

namespace FileOpsService.Models;

public class Employee
{
    [Required]
    public string? Name { get; set; }

    [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain numbers only.")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email address.")]
    public string? Email { get; set; }

    public string? CompanyName { get; set; }
}