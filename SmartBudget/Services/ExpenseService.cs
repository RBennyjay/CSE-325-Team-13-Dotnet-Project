using SmartBudget.DTOs;
using SmartBudget.Models;
using SmartBudget.Repositories;

namespace SmartBudget.Services;

public interface IExpenseService
{
    Task<List<ExpenseDto>> GetExpensesByUserAsync(string userId, int? month = null, int? year = null);
    Task<ExpenseDto?> GetExpenseByIdAsync(int id, string userId);
    Task<ExpenseDto> CreateExpenseAsync(string userId, CreateExpenseDto dto);
    Task<ExpenseDto> UpdateExpenseAsync(int id, string userId, CreateExpenseDto dto);
    Task DeleteExpenseAsync(int id, string userId);
    Task<decimal> GetTotalExpensesByUserAsync(string userId, int month, int year);
}

public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IRepository<Category> _categoryRepository;

    public ExpenseService(IRepository<Expense> expenseRepository, IRepository<Category> categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ExpenseDto>> GetExpensesByUserAsync(string userId, int? month = null, int? year = null)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var userExpenses = expenses.Where(e => e.UserId == userId).ToList();

        if (month.HasValue && year.HasValue)
        {
            userExpenses = userExpenses.Where(e =>
                e.Date.Month == month.Value && e.Date.Year == year.Value).ToList();
        }

        return userExpenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            CategoryId = e.CategoryId,
            CategoryName = e.Category?.Name ?? "Unknown",
            Amount = e.Amount,
            Description = e.Description,
            Date = e.Date,
            CreatedAt = e.CreatedAt
        }).OrderByDescending(e => e.Date).ToList();
    }

    public async Task<ExpenseDto?> GetExpenseByIdAsync(int id, string userId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense?.UserId != userId) return null;

        return new ExpenseDto
        {
            Id = expense.Id,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category?.Name ?? "Unknown",
            Amount = expense.Amount,
            Description = expense.Description,
            Date = expense.Date,
            CreatedAt = expense.CreatedAt
        };
    }

    public async Task<ExpenseDto> CreateExpenseAsync(string userId, CreateExpenseDto dto)
    {
        var expense = new Expense
        {
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Description = dto.Description,
            Date = dto.Date
        };

        var created = await _expenseRepository.AddAsync(expense);
        return new ExpenseDto
        {
            Id = created.Id,
            CategoryId = created.CategoryId,
            Amount = created.Amount,
            Description = created.Description,
            Date = created.Date,
            CreatedAt = created.CreatedAt
        };
    }

    public async Task<ExpenseDto> UpdateExpenseAsync(int id, string userId, CreateExpenseDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense?.UserId != userId) throw new UnauthorizedAccessException();

        expense.CategoryId = dto.CategoryId;
        expense.Amount = dto.Amount;
        expense.Description = dto.Description;
        expense.Date = dto.Date;
        expense.UpdatedAt = DateTime.UtcNow;

        var updated = await _expenseRepository.UpdateAsync(expense);
        return new ExpenseDto
        {
            Id = updated.Id,
            CategoryId = updated.CategoryId,
            Amount = updated.Amount,
            Description = updated.Description,
            Date = updated.Date,
            CreatedAt = updated.CreatedAt
        };
    }

    public async Task DeleteExpenseAsync(int id, string userId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense?.UserId != userId) throw new UnauthorizedAccessException();

        await _expenseRepository.DeleteAsync(id);
    }

    public async Task<decimal> GetTotalExpensesByUserAsync(string userId, int month, int year)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        return expenses
            .Where(e => e.UserId == userId && e.Date.Month == month && e.Date.Year == year)
            .Sum(e => e.Amount);
    }
}
