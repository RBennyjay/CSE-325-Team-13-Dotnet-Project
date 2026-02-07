using SmartBudget.Interfaces;
using SmartBudget.DTOs;
using SmartBudget.Models;

namespace SmartBudget.Interfaces;

public interface IExpenseService
{
    Task<List<ExpenseDto>> GetExpensesByUserAsync(string userId, int? month = null, int? year = null);
    Task<ExpenseDto?> GetExpenseByIdAsync(int id, string userId);
    Task<ExpenseDto> CreateExpenseAsync(string userId, CreateExpenseDto dto);
    Task<ExpenseDto> UpdateExpenseAsync(int id, string userId, CreateExpenseDto dto);
    Task DeleteExpenseAsync(int id, string userId);
    Task<decimal> GetTotalExpensesByUserAsync(string userId, int month, int year);
    Task<List<Category>> GetCategoriesAsync();
}