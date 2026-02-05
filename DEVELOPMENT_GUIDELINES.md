# Development Guidelines

## Code Style & Standards

### C# Conventions
```csharp
// Class names: PascalCase
public class UserService { }

// Method names: PascalCase
public async Task<UserDto> GetUserByIdAsync(int id) { }

// Private fields: _camelCase
private readonly IRepository<User> _repository;

// Properties: PascalCase
public int Id { get; set; }

// Constants: UPPER_CASE
public const int MaxNameLength = 100;
```

### Async/Await
- Always use `async` methods with `Task` return types
- Use `await` for I/O operations
- Suffix async methods with `Async`

```csharp
public async Task<UserDto> GetUserAsync(int id)
{
    var user = await _repository.GetByIdAsync(id);
    return new UserDto { /* map data */ };
}
```

### Null Safety
- Use null-coalescing operator
- Use null-conditional operator
- Validate inputs early

```csharp
// Good
var name = user?.Name ?? "Unknown";

// Good - early validation
if (user == null)
    throw new ArgumentNullException(nameof(user));
```

### Exception Handling
```csharp
// Specific exception types
try
{
    await _repository.DeleteAsync(id);
}
catch (InvalidOperationException ex)
{
    _logger.LogError(ex, "Failed to delete item");
    throw;
}
```

## Service Implementation Guidelines

### Creating a New Service

```csharp
namespace SmartBudget.Services;

public interface IMyFeatureService
{
    Task<MyFeatureDto> GetAsync(int id, string userId);
    Task<List<MyFeatureDto>> GetAllAsync(string userId);
    Task<MyFeatureDto> CreateAsync(string userId, MyFeatureDto dto);
    Task<MyFeatureDto> UpdateAsync(int id, string userId, MyFeatureDto dto);
    Task DeleteAsync(int id, string userId);
}

public class MyFeatureService : IMyFeatureService
{
    private readonly IRepository<MyFeature> _repository;

    public MyFeatureService(IRepository<MyFeature> repository)
    {
        _repository = repository;
    }

    public async Task<MyFeatureDto> GetAsync(int id, string userId)
    {
        var item = await _repository.GetByIdAsync(id);
        
        // Always check user ownership
        if (item == null || item.ApplicationUserId != userId)
            return null;

        return new MyFeatureDto { /* map */ };
    }

    // ... implement other methods
}
```

### Register in ServiceExtensions

```csharp
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add your service
        services.AddScoped<IMyFeatureService, MyFeatureService>();
        
        return services;
    }
}
```

## Blazor Component Guidelines

### Component Structure

```razor
@page "/myfeature"
@using SmartBudget.Services
@using SmartBudget.DTOs
@inject IMyFeatureService MyFeatureService

<PageTitle>My Feature</PageTitle>

<div class="container mt-4">
    <h1>My Feature</h1>
    
    @if (isLoading)
    {
        <LoadingSpinner />
    }
    else if (items != null && items.Any())
    {
        @foreach (var item in items)
        {
            <!-- Display item -->
        }
    }
    else
    {
        <EmptyStateComponent Message="No items found" />
    }
</div>

@code {
    private List<MyFeatureDto>? items;
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        // Get current user
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {
            items = await MyFeatureService.GetAllAsync(userId);
        }

        isLoading = false;
    }
}
```

### Best Practices

1. **Always check authentication**
   ```csharp
   var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
   if (userId == null) { /* handle */ }
   ```

2. **Use loading states**
   ```csharp
   private bool isLoading = true;
   // ... set to false when done
   ```

3. **Handle errors gracefully**
   ```csharp
   try
   {
       // operation
   }
   catch (Exception ex)
   {
       errorMessage = "An error occurred: " + ex.Message;
   }
   ```

4. **Use shared components**
   ```razor
   <AlertComponent AlertType="danger" Message="Error message" />
   <LoadingSpinner />
   ```

## Testing Guidelines

### Testing Services

```csharp
[TestFixture]
public class MyFeatureServiceTests
{
    private Mock<IRepository<MyFeature>> _repositoryMock;
    private MyFeatureService _service;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository<MyFeature>>();
        _service = new MyFeatureService(_repositoryMock.Object);
    }

    [Test]
    public async Task GetAsync_WithValidId_ReturnsDto()
    {
        // Arrange
        var myFeature = new MyFeature { Id = 1, ApplicationUserId = "user1" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(myFeature);

        // Act
        var result = await _service.GetAsync(1, "user1");

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(1, result.Id);
    }
}
```

## Database Operations

### Adding a Migration

```bash
cd CSE-325-Team-13-Dotnet-Project
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Updating DbContext

Always add new entities to `ApplicationDbContext`:

```csharp
public DbSet<MyFeature> MyFeatures { get; set; }
```

## Performance Considerations

1. **Lazy Loading**: Be cautious with lazy loading, use `.Include()` when needed
2. **Pagination**: Implement pagination for large datasets
3. **Caching**: Consider caching frequently accessed data
4. **Async All The Way**: Use async operations for I/O

## Security Guidelines

1. **Always validate user ownership**
   ```csharp
   if (entity.ApplicationUserId != userId)
       throw new UnauthorizedAccessException();
   ```

2. **Validate input data**
   ```csharp
   var errors = ValidationHelper.ValidateIncomeInput(source, amount, date);
   if (errors.Any())
       return error response
   ```

3. **Don't expose sensitive data in DTOs**
4. **Use HTTPS in production**
5. **Validate all user inputs**

## Logging

```csharp
private readonly ILogger<MyService> _logger;

public MyService(ILogger<MyService> logger)
{
    _logger = logger;
}

// Use appropriate log levels
_logger.LogInformation("Operation completed");
_logger.LogWarning("Unexpected condition");
_logger.LogError(ex, "Operation failed");
```

## Commit Messages

Follow conventional commits:
- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation
- `style:` Code style
- `refactor:` Code refactoring
- `test:` Tests
- `chore:` Maintenance

Example: `feat: add income tracking service`

## Code Review Checklist

- [ ] Code follows style guidelines
- [ ] All public methods have documentation
- [ ] Error handling is appropriate
- [ ] User authorization is checked
- [ ] Tests are included
- [ ] No hardcoded values (use Constants)
- [ ] Async/await is used correctly
- [ ] DTOs are used for data transfer
- [ ] Null safety is handled
