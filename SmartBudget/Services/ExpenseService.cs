using SmartBudget.DTOs;
using SmartBudget.Models;
using SmartBudget.Repositories;

using SmartBudget.Interfaces;

namespace SmartBudget.Services;


public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IRepository<Category> _categoryRepository;

    public ExpenseService(IRepository<Expense> expenseRepository, IRepository<Category> categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
    }

    // public async Task<List<ExpenseDto>> GetExpensesByUserAsync(string userId, int? month = null, int? year = null)
    // {
    //     var expenses = await _expenseRepository.GetAllAsync();
    //     var categories = await _categoryRepository.GetAllAsync(); // Fetch categories to join manually

    //     var userExpenses = expenses.Where(e => e.UserId == userId).ToList();

    //     if (month.HasValue && year.HasValue)
    //     {
    //         userExpenses = userExpenses.Where(e =>
    //             e.Date.Month == month.Value && e.Date.Year == year.Value).ToList();
    //     }

    //     return userExpenses.Select(e =>
    //     {
    //         var dto = MapToDto(e);
    //         // Manually assign the category name if the Include failed
    //         dto.CategoryName = categories.FirstOrDefault(c => c.Id == e.CategoryId)?.Name ?? "Unknown";
    //         return dto;
    //     }).OrderByDescending(e => e.Date).ToList();
    // }

    // public async Task<List<ExpenseDto>> GetExpensesByUserAsync(
    // string? userId,
    // int? month = null,
    // int? year = null)
    // {
    //     var expenses = await _expenseRepository.GetAllAsync();
    //     var categories = await _categoryRepository.GetAllAsync();

    //     // Build a fast lookup for category names
    //     var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

    //     // Allow null userId (dev mode) AND real user IDs
    //     // var userExpenses = expenses
    //     //     .Where(e => e.UserId == userId)
    //     //     .ToList();

    //     var userExpenses = string.IsNullOrWhiteSpace(userId)
    // ? expenses.ToList()
    // : expenses.Where(e => e.UserId == userId).ToList();


    //     if (month.HasValue && year.HasValue)
    //     {
    //         userExpenses = userExpenses
    //             .Where(e =>
    //                 e.Date.Month == month.Value &&
    //                 e.Date.Year == year.Value)
    //             .ToList();
    //     }

    //     return userExpenses
    //         .OrderByDescending(e => e.Date)
    //         .Select(e =>
    //         {
    //             var dto = MapToDto(e);
    //             dto.CategoryName = categoryMap.GetValueOrDefault(e.CategoryId, "Unknown");
    //             return dto;
    //         })
    //         .ToList();
    // }



    public async Task<List<ExpenseDto>> GetExpensesByUserAsync(
    string? userId,
    int? month = null,
    int? year = null)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();
        var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

        // 1. Loosen the UserID check for Dev Mode
        var query = expenses.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(userId) && userId != "dev-user-123")
        {
            query = query.Where(e => e.UserId == userId);
        }

        // 2. Date Filtering - Check if this is the reason for 0 results
        if (month.HasValue && year.HasValue)
        {
            query = query.Where(e =>
                e.Date.Month == month.Value &&
                e.Date.Year == year.Value);
        }

        var result = query
            .OrderByDescending(e => e.Date)
            .Select(e =>
            {
                var dto = MapToDto(e);
                dto.CategoryName = categoryMap.GetValueOrDefault(e.CategoryId, "Unknown");
                return dto;
            })
            .ToList();

        // 3. Debugging - This will tell you exactly why count is 0
        Console.WriteLine($"Service: Returning {result.Count} expenses for User: {userId}, Month: {month}, Year: {year}");

        return result;
    }


    public async Task<ExpenseDto?> GetExpenseByIdAsync(int id, string userId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense?.UserId != userId) return null;

        return MapToDto(expense);
    }

    // public async Task<ExpenseDto> CreateExpenseAsync(string userId, CreateExpenseDto dto)
    // public async Task<ExpenseDto> CreateExpenseAsync(string? userId, CreateExpenseDto dto)

    // {
    //     var expense = new Expense
    //     {
    //         UserId = userId,
    //         CategoryId = dto.CategoryId,
    //         Amount = dto.Amount,
    //         Description = dto.Description,
    //         Date = dto.Date,
    //         CreatedAt = DateTime.UtcNow // Ensure timestamp is set
    //     };

    //     var created = await _expenseRepository.AddAsync(expense);

    //     // Fetch the category name so the UI updates correctly
    //     var category = await _categoryRepository.GetByIdAsync(created.CategoryId);
    //     var result = MapToDto(created);
    //     result.CategoryName = category?.Name ?? "Unknown";

    //     return result;
    // }

    public async Task<ExpenseDto> CreateExpenseAsync(string? userId, CreateExpenseDto dto)
    {
        // LOG THIS: It will show up in your terminal
        Console.WriteLine($"DEBUG: Saving Expense - User: {userId}, Category: {dto.CategoryId}, Amount: {dto.Amount}");

        if (dto.CategoryId <= 0)
        {
            throw new Exception("Category ID must be greater than 0.");
        }

        var expense = new Expense
        {
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Description = dto.Description,
            Date = dto.Date,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _expenseRepository.AddAsync(expense);

        var category = await _categoryRepository.GetByIdAsync(created.CategoryId);
        var result = MapToDto(created);
        result.CategoryName = category?.Name ?? "Unknown";

        return result;
    }


    public async Task<ExpenseDto> UpdateExpenseAsync(int id, string userId, CreateExpenseDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense?.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to edit this expense.");

        expense.CategoryId = dto.CategoryId;
        expense.Amount = dto.Amount;
        expense.Description = dto.Description;
        expense.Date = dto.Date;
        expense.UpdatedAt = DateTime.UtcNow;

        var updated = await _expenseRepository.UpdateAsync(expense);
        return MapToDto(updated);
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

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.OrderBy(c => c.Name).ToList();
    }

    // Helper method to keep code DRY (Don't Repeat Yourself)
    // private ExpenseDto MapToDto(Expense e) => new ExpenseDto
    // {
    //     Id = e.Id,
    //     CategoryId = e.CategoryId,
    //     CategoryName = e.Category?.Name ?? "Unknown",
    //     Amount = e.Amount,
    //     Description = e.Description,
    //     Date = e.Date,
    //     CreatedAt = e.CreatedAt
    // };

    private ExpenseDto MapToDto(Expense e) => new ExpenseDto
    {
        Id = e.Id,
        CategoryId = e.CategoryId,
        // Use CategoryId if Name is null to at least see SOMETHING
        CategoryName = e.Category?.Name ?? $"Cat ID: {e.CategoryId}",
        Amount = e.Amount,
        Description = e.Description,
        Date = e.Date
    };
}