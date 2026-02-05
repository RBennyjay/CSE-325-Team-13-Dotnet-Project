namespace SmartBudget.Models;

public class Budget
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal LimitAmount { get; set; }
    public int Month { get; set; } // 1-12
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ApplicationUser? User { get; set; }
    public Category? Category { get; set; }
}
