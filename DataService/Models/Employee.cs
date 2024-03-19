using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataService.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public int EmployeeId { get; set; }

    [Required]
    public string? Name { get; set; }

    [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain numbers only.")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email address.")]
    public string? Email { get; set; }
    public string? CompanyName { get; set; }
}