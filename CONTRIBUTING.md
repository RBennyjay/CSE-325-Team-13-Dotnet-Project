# Contributing to Smart Budget

Thank you for contributing to the Smart Budget project! This document outlines our development guidelines, coding standards, and workflow.

## Code Standards

### C# Naming Conventions

- **Classes**: PascalCase (e.g., `ApplicationUser`, `ExpenseService`)
- **Methods**: PascalCase (e.g., `GetExpensesByUserAsync`)
- **Properties**: PascalCase (e.g., `Amount`, `Description`)
- **Private fields**: camelCase with underscore prefix (e.g., `_expenseRepository`)
- **Local variables**: camelCase (e.g., `userExpenses`, `totalAmount`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_BUDGET_LIMIT`)

### File Organization

- One public class per file (except nested classes)
- File name matches class name
- Related classes grouped in folders (Models/, Services/, Repositories/, etc.)

### Async/Await

- Always use `async/await` for I/O operations
- Method names should end with `Async` if they're async (e.g., `GetExpensesByUserAsync`)
- Avoid `.Result` or `.Wait()` - use `await` instead

### Error Handling

```csharp
// DO
try
{
    var expense = await _expenseRepository.GetByIdAsync(id);
    if (expense?.UserId != userId)
    {
        throw new UnauthorizedAccessException("User not authorized to access this expense.");
    }
}
catch (UnauthorizedAccessException ex)
{
    _logger.LogWarning($"Unauthorized access attempt: {ex.Message}");
    throw;
}

// DON'T
try
{
    var expense = await _expenseRepository.GetByIdAsync(id);
}
catch { }  // Don't swallow exceptions
```

### Dependency Injection

- Use constructor injection for all dependencies
- Register services in `Program.cs` using the DI container
- Use interfaces for abstraction

```csharp
public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _repository;
    
    public ExpenseService(IRepository<Expense> repository)
    {
        _repository = repository;
    }
}
```

## Git Workflow

### Branch Naming
- `feature/feature-name` - New feature
- `bugfix/bug-name` - Bug fix
- `docs/documentation-update` - Documentation
- `refactor/refactoring-name` - Code refactoring

### Commit Messages
Format: `[Type] Brief description`

Examples:
- `[feat] Add expense category filtering`
- `[fix] Correct budget calculation logic`
- `[docs] Update database schema documentation`
- `[refactor] Extract common validation logic`

Types:
- `feat` - New feature
- `fix` - Bug fix
- `docs` - Documentation
- `refactor` - Code refactoring
- `test` - Tests
- `chore` - Dependencies, build configuration

### Pull Requests
1. Create a new branch from `main`
2. Make your changes
3. Push to your branch
4. Create a PR with a clear description
5. Request review from team members
6. Address feedback
7. Merge when approved

## Testing

- Write unit tests for business logic
- Test services before integration
- Use test data and mock repositories
- Run tests locally before pushing: `dotnet test`

## Security Guidelines

- Never commit sensitive information (passwords, API keys)
- Use `appsettings.json` for configuration
- Use `appsettings.Development.json` for development secrets (get added to .gitignore)
- Validate all user input
- Use parameterized queries for database operations (EF Core handles this)
- Hash passwords using ASP.NET Core Identity

## Documentation

- Add XML comments to public methods and classes
- Update README.md if adding features
- Document complex algorithms or business logic
- Update DEVELOPMENT.md if changing setup process

Example XML documentation:

```csharp
/// <summary>
/// Retrieves all expenses for a user in a specific month.
/// </summary>
/// <param name="userId">The ID of the user.</param>
/// <param name="month">The month (1-12).</param>
/// <param name="year">The year.</param>
/// <returns>A list of expenses for the specified month.</returns>
public async Task<List<ExpenseDto>> GetExpensesByMonthAsync(string userId, int month, int year)
{
    // Implementation
}
```

## Database Changes

1. Update the model in `Models/`
2. Modify `ApplicationDbContext` if needed
3. Create a migration: `dotnet ef migrations add <DescriptiveName>`
4. Review the migration file
5. Test locally: `dotnet ef database update`
6. Commit migration files with your changes

## Performance Considerations

- Use `.ToListAsync()` or `.FirstOrDefaultAsync()` for async database queries
- Avoid N+1 query problems - use `.Include()` for related data
- Use pagination for large datasets
- Cache frequently accessed data if needed

## Debugging

### Using Visual Studio Code
- Set breakpoints in code
- Use `F5` to launch with debugging
- Use debug console to evaluate expressions

### Logging
```csharp
// Inject ILogger<T>
public class ExpenseService
{
    private readonly ILogger<ExpenseService> _logger;
    
    public ExpenseService(ILogger<ExpenseService> logger)
    {
        _logger = logger;
    }
    
    public async Task DoSomethingAsync()
    {
        _logger.LogInformation("Processing expense...");
        _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);
    }
}
```

## Code Review Checklist

Before submitting a PR, ensure:
- [ ] Code follows naming conventions
- [ ] No hardcoded values or magic numbers
- [ ] Error handling is implemented
- [ ] Async/await is used properly
- [ ] No unused variables or imports
- [ ] Comments are clear and helpful
- [ ] Database migration created if needed
- [ ] Tests pass locally
- [ ] No sensitive data is committed

## Questions or Issues?

- Ask in the team Slack/Discord
- Create an issue on GitHub for bugs
- Comment on relevant Trello cards
- Reference the [DEVELOPMENT.md](./DEVELOPMENT.md) guide

## License

This project is for educational purposes as part of CSE-325 at BYU-Pathway Worldwide.
