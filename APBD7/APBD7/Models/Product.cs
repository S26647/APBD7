using System.ComponentModel.DataAnnotations;

namespace APBD7.Models;

public class Product
{
    [Required]
    public int IdProduct { get; set; }
    [MaxLength(200)]
    public string? Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [Required]
    [RegularExpression(@"^\d{1,25}\.\d{2}$")]
    public double Price { get; set; }
}