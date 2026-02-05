using SmartBudget.DTOs;
using SmartBudget.Models;

namespace SmartBudget.Interfaces
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDto>> GetUserIncomeAsync(string userId);
        Task<IncomeDto?> GetIncomeByIdAsync(int incomeId, string userId);
        Task<Income> CreateIncomeAsync(IncomeDto incomeDto, string userId);
        Task<Income?> UpdateIncomeAsync(int incomeId, IncomeDto incomeDto, string userId);
        Task<bool> DeleteIncomeAsync(int incomeId, string userId);
        Task<decimal> GetTotalIncomeAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
