# Smart Budget - API & Service Reference

## Overview

This document provides reference for the services and their methods available in the Smart Budget application.

---

## Service Layer Architecture

All business logic is encapsulated in services following the Service Layer pattern. Services are injected via dependency injection and handle:
- Data validation
- Business rule enforcement
- Authorization checks
- Transformation to/from DTOs
- Error handling

---

## 📊 ExpenseService

Manages all expense-related operations with user-specific data access control.

### Interface: `IExpenseService`

#### GetExpensesByUserAsync
```csharp
Task<List<ExpenseDto>> GetExpensesByUserAsync(
    string userId,
    int? month = null,
    int? year = null
)
```
- **Purpose**: Retrieve user's expenses with optional month/year filtering
- **Returns**: List of ExpenseDto ordered by date (newest first)
- **Auth**: Returns only expenses for specified user

#### GetExpenseByIdAsync
```csharp
Task<ExpenseDto?> GetExpenseByIdAsync(int id, string userId)
```
- **Purpose**: Get a specific expense by ID
- **Returns**: ExpenseDto or null if not found
- **Auth**: Validates user ownership

#### CreateExpenseAsync
```csharp
Task<ExpenseDto> CreateExpenseAsync(
    string userId,
    CreateExpenseDto dto
)
```
- **Purpose**: Create a new expense record
- **Input**: CreateExpenseDto with amount, description, date, category
- **Returns**: Created ExpenseDto with ID
- **Auth**: Associates with current user

#### UpdateExpenseAsync
```csharp
Task<ExpenseDto> UpdateExpenseAsync(
    int id,
    string userId,
    CreateExpenseDto dto
)
```
- **Purpose**: Update an existing expense
- **Auth**: Throws UnauthorizedAccessException if user doesn't own expense
- **Returns**: Updated ExpenseDto

#### DeleteExpenseAsync
```csharp
Task DeleteExpenseAsync(int id, string userId)
```
- **Purpose**: Delete an expense record
- **Auth**: Throws UnauthorizedAccessException if unauthorized

#### GetTotalExpensesByUserAsync
```csharp
Task<decimal> GetTotalExpensesByUserAsync(
    string userId,
    int month,
    int year
)
```
- **Purpose**: Calculate total expenses for a specific month/year
- **Returns**: Sum of all expenses as decimal
- **Use Case**: For budget comparison and analytics

---

## 🏷️ CategoryService

Manages expense categories for organizing and grouping expenses.

### Interface: `ICategoryService`

#### GetCategoriesByUserAsync
```csharp
Task<List<CategoryDto>> GetCategoriesByUserAsync(string userId)
```
- **Purpose**: Get all categories for a user
- **Returns**: List of CategoryDto ordered alphabetically
- **Use Case**: Populate dropdown menus for expense creation

#### GetCategoryByIdAsync
```csharp
Task<CategoryDto?> GetCategoryByIdAsync(int id, string userId)
```
- **Purpose**: Get a specific category
- **Returns**: CategoryDto or null
- **Auth**: Validates ownership

#### CreateCategoryAsync
```csharp
Task<CategoryDto> CreateCategoryAsync(
    string userId,
    CreateCategoryDto dto
)
```
- **Purpose**: Create a new custom category
- **Input**: Name, optional description, hex color
- **Returns**: Created CategoryDto with ID
- **Example Colors**: #FF6B6B (red), #4ECDC4 (teal), #45B7D1 (blue)

#### UpdateCategoryAsync
```csharp
Task<CategoryDto> UpdateCategoryAsync(
    int id,
    string userId,
    CreateCategoryDto dto
)
```
- **Purpose**: Modify category details
- **Auth**: Validates ownership
- **Returns**: Updated CategoryDto

#### DeleteCategoryAsync
```csharp
Task DeleteCategoryAsync(int id, string userId)
```
- **Purpose**: Remove a category
- **Note**: Consider cascading effect on expenses

---

## 🎯 Generic Repository Pattern

All data access is abstracted through a generic repository implementing the Repository pattern.

### Interface: `IRepository<T>`

#### GetByIdAsync
```csharp
Task<T?> GetByIdAsync(int id)
```

#### GetAllAsync
```csharp
Task<IEnumerable<T>> GetAllAsync()
```

#### AddAsync
```csharp
Task<T> AddAsync(T entity)
```

#### UpdateAsync
```csharp
Task<T> UpdateAsync(T entity)
```

#### DeleteAsync
```csharp
Task DeleteAsync(int id)
```

#### SaveChangesAsync
```csharp
Task SaveChangesAsync()
```

---

## 💰 Data Transfer Objects (DTOs)

### IncomeDto / CreateIncomeDto
```csharp
public class IncomeDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### ExpenseDto / CreateExpenseDto
```csharp
public class ExpenseDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### CategoryDto / CreateCategoryDto
```csharp
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### BudgetDto / CreateBudgetDto
```csharp
public class BudgetDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal LimitAmount { get; set; }
    public int Month { get; set; }      // 1-12
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## 🔐 Authorization & Security

### User Isolation
All services enforce user isolation to prevent unauthorized data access:

```csharp
// Example: Verify user owns the resource
public async Task<ExpenseDto?> GetExpenseByIdAsync(int id, string userId)
{
    var expense = await _expenseRepository.GetByIdAsync(id);
    if (expense?.UserId != userId) 
        return null;  // Or throw UnauthorizedAccessException
    
    return MapToExpenseDto(expense);
}
```

### Authentication
- Uses ASP.NET Core Identity
- User ID is injected from authenticated claims
- All endpoints should require authentication

---

## 🚀 Dependency Injection Setup

Services are registered in `Program.cs`:

```csharp
// Add repository services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Future service registrations:
// builder.Services.AddScoped<IExpenseService, ExpenseService>();
// builder.Services.AddScoped<ICategoryService, CategoryService>();
// builder.Services.AddScoped<IIncomeService, IncomeService>();
// builder.Services.AddScoped<IBudgetService, BudgetService>();
```

### Usage in Components/Controllers
```csharp
public class YourComponent
{
    [Inject]
    public IExpenseService ExpenseService { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        var expenses = await ExpenseService.GetExpensesByUserAsync(userId);
    }
}
```

---

## 📋 Future Services

Services planned for implementation:

1. **IncomeService** - Income tracking and summaries
2. **BudgetService** - Budget management and alerts
3. **ReportService** - Analytics and report generation
4. **StatisticsService** - Spending analysis and trends
5. **NotificationService** - Budget alerts and reminders

---

## 🔄 Common Patterns

### Error Handling
```csharp
try
{
    var expense = await _expenseRepository.GetByIdAsync(id);
    if (expense?.UserId != userId)
        throw new UnauthorizedAccessException("Not authorized");
}
catch (UnauthorizedAccessException ex)
{
    _logger.LogWarning($"Unauthorized: {ex.Message}");
    throw;
}
```

### Async Operations
```csharp
// Always use async for I/O
public async Task<List<ExpenseDto>> GetExpensesAsync()
{
    // Bad: var list = _repository.GetAll().ToList();
    // Good:
    var expenses = await _expenseRepository.GetAllAsync();
    return expenses.Select(MapToDto).ToList();
}
```

### Date Handling
```csharp
// Use UTC for storage
public DateTime Date { get; set; } = DateTime.UtcNow;

// Convert to local time in UI layer
var localDate = date.ToLocalTime();
```

---

## 📞 API Consumption Example

```csharp
// In a Blazor component
@if (expenses != null)
{
    @foreach (var expense in expenses)
    {
        <div class="expense-item">
            <p>@expense.CategoryName</p>
            <p>@expense.Amount.ToString("C")</p>
            <p>@expense.Date.ToShortDateString()</p>
        </div>
    }
}

@code {
    private List<ExpenseDto> expenses = null!;
    
    [Inject]
    public IExpenseService ExpenseService { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        var userId = GetCurrentUserId(); // From auth
        expenses = await ExpenseService.GetExpensesByUserAsync(userId);
    }
}
```

---

**Last Updated:** February 5, 2026
**Version:** 1.0
