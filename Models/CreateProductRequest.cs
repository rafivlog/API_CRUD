using System.ComponentModel.DataAnnotations;

namespace APICRUD.Models;

public class CreateProductRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string ProductName { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal Price { get; set; }
}
