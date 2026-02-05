using Microsoft.AspNetCore.Identity;

namespace SmartBudget.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    
    // Navigation properties
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
