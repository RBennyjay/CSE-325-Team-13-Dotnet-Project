using SmartBudget.DTOs;
using SmartBudget.Models;
using SmartBudget.Repositories;

namespace SmartBudget.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesByUserAsync(string userId);
    Task<CategoryDto?> GetCategoryByIdAsync(int id, string userId);
    Task<CategoryDto> CreateCategoryAsync(string userId, CreateCategoryDto dto);
    Task<CategoryDto> UpdateCategoryAsync(int id, string userId, CreateCategoryDto dto);
    Task DeleteCategoryAsync(int id, string userId);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetCategoriesByUserAsync(string userId)
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories
            .Where(c => c.UserId == userId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Color = c.Color,
                CreatedAt = c.CreatedAt
            })
            .OrderBy(c => c.Name)
            .ToList();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, string userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category?.UserId != userId) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(string userId, CreateCategoryDto dto)
    {
        var category = new Category
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color
        };

        var created = await _categoryRepository.AddAsync(category);
        return new CategoryDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            Color = created.Color,
            CreatedAt = created.CreatedAt
        };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, string userId, CreateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category?.UserId != userId) throw new UnauthorizedAccessException();

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Color = dto.Color;
        category.UpdatedAt = DateTime.UtcNow;

        var updated = await _categoryRepository.UpdateAsync(category);
        return new CategoryDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            Color = updated.Color,
            CreatedAt = updated.CreatedAt
        };
    }

    public async Task DeleteCategoryAsync(int id, string userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category?.UserId != userId) throw new UnauthorizedAccessException();

        await _categoryRepository.DeleteAsync(id);
    }
}
