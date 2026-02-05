using SmartBudget.DTOs;
using SmartBudget.Models;

namespace SmartBudget.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetDto>> GetUserBudgetsAsync(string userId);
        Task<BudgetDto?> GetBudgetByIdAsync(int budgetId, string userId);
        Task<Budget> CreateBudgetAsync(BudgetDto budgetDto, string userId);
        Task<Budget?> UpdateBudgetAsync(int budgetId, BudgetDto budgetDto, string userId);
        Task<bool> DeleteBudgetAsync(int budgetId, string userId);
        Task<decimal> GetRemainingBudgetAsync(int budgetId, string userId);
        Task<bool> IsBudgetExceededAsync(int budgetId, string userId);
    }
}
