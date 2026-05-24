using System.ComponentModel.DataAnnotations;

namespace ProductStore.Api.DTOs;

public class ProductCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string SKU { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
}