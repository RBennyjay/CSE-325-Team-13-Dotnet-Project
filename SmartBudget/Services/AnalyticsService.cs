using SmartBudget.Models;
using SmartBudget.Repositories;
using SmartBudget.Interfaces;

namespace SmartBudget.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IRepository<Income> _incomeRepository;
    private readonly IRepository<Budget> _budgetRepository;
    private readonly IRepository<Category> _categoryRepository;

    public AnalyticsService(
        IRepository<Expense> expenseRepository,
        IRepository<Income> incomeRepository,
        IRepository<Budget> budgetRepository,
        IRepository<Category> categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _incomeRepository = incomeRepository;
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();

        var query = expenses.Where(e => e.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var grouped = query
            .GroupBy(e => e.CategoryId)
            .Select(g => new
            {
                CategoryId = g.Key,
                Total = g.Sum(e => e.Amount)
            });

        var result = new Dictionary<string, decimal>();
        foreach (var item in grouped)
        {
            var category = categories.FirstOrDefault(c => c.Id == item.CategoryId);
            var categoryName = category?.Name ?? "Unknown";
            result[categoryName] = item.Total;
        }

        return result;
    }

    public async Task<decimal> GetTotalExpensesAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var query = expenses.Where(e => e.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        return query.Sum(e => e.Amount);
    }

    public async Task<decimal> GetTotalIncomeAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var incomes = await _incomeRepository.GetAllAsync();
        var query = incomes.Where(i => i.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(i => i.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(i => i.Date <= endDate.Value);

        return query.Sum(i => i.Amount);
    }

    public async Task<decimal> GetNetBalanceAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var income = await GetTotalIncomeAsync(userId, startDate, endDate);
        var expenses = await GetTotalExpensesAsync(userId, startDate, endDate);
        return income - expenses;
    }

    public async Task<Dictionary<string, object>> GetMonthlyTrendsAsync(string userId, int months = 12)
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var incomes = await _incomeRepository.GetAllAsync();

        var userExpenses = expenses.Where(e => e.UserId == userId).ToList();
        var userIncomes = incomes.Where(i => i.UserId == userId).ToList();

        var result = new Dictionary<string, object>();

        for (int i = months - 1; i >= 0; i--)
        {
            var date = DateTime.Now.AddMonths(-i);
            var monthKey = date.ToString("yyyy-MM");

            var monthExpenses = userExpenses
                .Where(e => e.Date.Year == date.Year && e.Date.Month == date.Month)
                .Sum(e => e.Amount);

            var monthIncomes = userIncomes
                .Where(e => e.Date.Year == date.Year && e.Date.Month == date.Month)
                .Sum(e => e.Amount);

            result[monthKey] = new
            {
                Income = monthIncomes,
                Expenses = monthExpenses,
                Balance = monthIncomes - monthExpenses
            };
        }

        return result;
    }

    public async Task<Dictionary<string, decimal>> GetBudgetVsActualAsync(string userId)
    {
        var budgets = await _budgetRepository.GetAllAsync();
        var expenses = await _expenseRepository.GetAllAsync();

        var userBudgets = budgets.Where(b => b.UserId == userId).ToList();
        var userExpenses = expenses.Where(e => e.UserId == userId).ToList();

        var result = new Dictionary<string, decimal>();

        foreach (var budget in userBudgets)
        {
            var actual = userExpenses
                .Where(e => e.CategoryId == budget.CategoryId &&
                           e.Date.Month == budget.Month &&
                           e.Date.Year == budget.Year)
                .Sum(e => e.Amount);

            var key = $"Budget_{budget.Id}";
            result[key] = actual > budget.LimitAmount ? -1 : (budget.LimitAmount - actual);
        }

        return result;
    }
}
