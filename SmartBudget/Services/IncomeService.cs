using SmartBudget.DTOs;
using SmartBudget.Models;
using SmartBudget.Repositories;
using SmartBudget.Interfaces;

namespace SmartBudget.Services;

public class IncomeService : IIncomeService
{
    private readonly IRepository<Income> _incomeRepository;

    public IncomeService(IRepository<Income> incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public async Task<IEnumerable<IncomeDto>> GetUserIncomeAsync(string userId)
    {
        var incomes = await _incomeRepository.GetAllAsync();
        return incomes
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.Date)
            .Select(i => new IncomeDto
            {
                Id = i.Id,
                Amount = i.Amount,
                Source = i.Source,
                Date = i.Date,
                Description = i.Description
            })
            .ToList();
    }

    public async Task<IncomeDto?> GetIncomeByIdAsync(int id, string userId)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        
        if (income == null || income.UserId != userId)
            return null;

        return new IncomeDto
        {
            Id = income.Id,
            Amount = income.Amount,
            Source = income.Source,
            Date = income.Date,
            Description = income.Description
        };
    }

    public async Task<Income> CreateIncomeAsync(IncomeDto incomeDto, string userId)
    {
        var income = new Income
        {
            Amount = incomeDto.Amount,
            Source = incomeDto.Source,
            Date = incomeDto.Date,
            Description = incomeDto.Description,
            UserId = userId
        };

        await _incomeRepository.AddAsync(income);
        await _incomeRepository.SaveChangesAsync();

        return income;
    }

    public async Task<Income?> UpdateIncomeAsync(int incomeId, IncomeDto incomeDto, string userId)
    {
        var income = await _incomeRepository.GetByIdAsync(incomeId);
        
        if (income == null || income.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to update this income.");

        income.Amount = incomeDto.Amount;
        income.Source = incomeDto.Source;
        income.Date = incomeDto.Date;
        income.Description = incomeDto.Description;

        await _incomeRepository.UpdateAsync(income);
        await _incomeRepository.SaveChangesAsync();

        return income;
    }

    public async Task<bool> DeleteIncomeAsync(int incomeId, string userId)
    {
        var income = await _incomeRepository.GetByIdAsync(incomeId);
        
        if (income == null || income.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to delete this income.");

        await _incomeRepository.DeleteAsync(incomeId);
        await _incomeRepository.SaveChangesAsync();
        
        return true;
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
}
