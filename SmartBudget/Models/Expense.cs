namespace SmartBudget.Models;

public class Expense
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ApplicationUser? User { get; set; }
    public Category? Category { get; set; }
}
