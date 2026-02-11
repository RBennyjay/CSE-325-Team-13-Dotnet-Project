namespace SmartBudget.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
