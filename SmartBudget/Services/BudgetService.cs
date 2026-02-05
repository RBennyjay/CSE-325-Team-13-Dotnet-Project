using SmartBudget.DTOs;
using SmartBudget.Models;
using SmartBudget.Repositories;
using SmartBudget.Interfaces;

namespace SmartBudget.Services;

public class BudgetService : IBudgetService
{
    private readonly IRepository<Budget> _budgetRepository;
    private readonly IRepository<Expense> _expenseRepository;

    public BudgetService(IRepository<Budget> budgetRepository, IRepository<Expense> expenseRepository)
    {
        _budgetRepository = budgetRepository;
        _expenseRepository = expenseRepository;
    }

    public async Task<IEnumerable<BudgetDto>> GetUserBudgetsAsync(string userId)
    {
        var budgets = await _budgetRepository.GetAllAsync();
        return budgets
            .Where(b => b.UserId == userId)
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                CategoryId = b.CategoryId,
                LimitAmount = b.LimitAmount,
                Month = b.Month,
                Year = b.Year
            })
            .ToList();
    }

    public async Task<BudgetDto?> GetBudgetByIdAsync(int budgetId, string userId)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId);
        
        if (budget == null || budget.UserId != userId)
            return null;

        return new BudgetDto
        {
            Id = budget.Id,
            CategoryId = budget.CategoryId,
            LimitAmount = budget.LimitAmount,
            Month = budget.Month,
            Year = budget.Year
        };
    }

    public async Task<Budget> CreateBudgetAsync(BudgetDto budgetDto, string userId)
    {
        var budget = new Budget
        {
            CategoryId = budgetDto.CategoryId,
            LimitAmount = budgetDto.LimitAmount,
            Month = budgetDto.Month,
            Year = budgetDto.Year,
            UserId = userId
        };

        await _budgetRepository.AddAsync(budget);
        await _budgetRepository.SaveChangesAsync();

        return budget;
    }

    public async Task<Budget?> UpdateBudgetAsync(int budgetId, BudgetDto budgetDto, string userId)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId);
        
        if (budget == null || budget.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to update this budget.");

        budget.LimitAmount = budgetDto.LimitAmount;
        budget.Month = budgetDto.Month;
        budget.Year = budgetDto.Year;

        await _budgetRepository.UpdateAsync(budget);
        await _budgetRepository.SaveChangesAsync();

        return budget;
    }

    public async Task<bool> DeleteBudgetAsync(int budgetId, string userId)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId);
        
        if (budget == null || budget.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to delete this budget.");

        await _budgetRepository.DeleteAsync(budgetId);
        await _budgetRepository.SaveChangesAsync();
        
        return true;
    }

    public async Task<decimal> GetRemainingBudgetAsync(int budgetId, string userId)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId);
        
        if (budget == null || budget.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to view this budget.");

        var expenses = await _expenseRepository.GetAllAsync();
        var spent = expenses
            .Where(e => e.CategoryId == budget.CategoryId && 
                       e.Date.Month == budget.Month && 
                       e.Date.Year == budget.Year &&
                       e.UserId == userId)
            .Sum(e => e.Amount);

        return budget.LimitAmount - spent;
    }

    public async Task<bool> IsBudgetExceededAsync(int budgetId, string userId)
    {
        var remaining = await GetRemainingBudgetAsync(budgetId, userId);
        return remaining < 0;
    }
}
