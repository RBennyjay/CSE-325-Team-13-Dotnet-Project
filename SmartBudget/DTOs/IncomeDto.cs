namespace SmartBudget.DTOs;

public class CreateIncomeDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}

public class IncomeDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
}
