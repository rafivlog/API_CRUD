using System.ComponentModel.DataAnnotations;

namespace APICRUD.Models;

public class UpdateCustomerRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CustomerCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150, MinimumLength = 1)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    public bool IsActive { get; set; } = true;
}
