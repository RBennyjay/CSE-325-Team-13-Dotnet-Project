namespace SmartBudget.Interfaces
{
    public interface IAnalyticsService
    {
        Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalExpensesAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalIncomeAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetNetBalanceAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Dictionary<string, object>> GetMonthlyTrendsAsync(string userId, int months = 12);
        Task<Dictionary<string, decimal>> GetBudgetVsActualAsync(string userId);
    }
}
