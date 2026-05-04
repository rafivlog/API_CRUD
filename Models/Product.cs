namespace APICRUD.Models;

public class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; }
}
