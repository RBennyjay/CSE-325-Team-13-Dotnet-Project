# Smart Budget - Project Structure Documentation

## Overview
This document describes the Blazor application structure for the Smart Budget project, following clean architecture and SOLID principles.

## Directory Structure

```
SmartBudget/
├── Components/                 # Blazor UI Components
│   ├── Layout/                 # Layout components
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor
│   │   └── ReconnectModal.razor
│   ├── Pages/                  # Page components (routable)
│   │   ├── Home.razor
│   │   ├── Income.razor
│   │   ├── Expenses.razor
│   │   ├── Budgets.razor
│   │   ├── Analytics.razor
│   │   └── ...
│   ├── Shared/                 # Reusable UI components
│   │   ├── AlertComponent.razor
│   │   ├── LoadingSpinner.razor
│   │   ├── EmptyStateComponent.razor
│   │   └── TransactionFormComponent.razor
│   ├── App.razor
│   ├── Routes.razor
│   └── _Imports.razor
│
├── Models/                      # Domain models (entities)
│   ├── ApplicationUser.cs
│   ├── Budget.cs
│   ├── Category.cs
│   ├── Expense.cs
│   └── Income.cs
│
├── DTOs/                        # Data Transfer Objects
│   ├── BudgetDto.cs
│   ├── CategoryDto.cs
│   ├── ExpenseDto.cs
│   └── IncomeDto.cs
│
├── Data/                        # Database context and migrations
│   ├── ApplicationDbContext.cs
│   └── Migrations/
│       └── [Migration files]
│
├── Services/                    # Business logic layer
│   ├── CategoryService.cs
│   ├── ExpenseService.cs
│   ├── IncomeService.cs
│   ├── BudgetService.cs
│   └── AnalyticsService.cs
│
├── Repositories/                # Data access layer
│   ├── IRepository.cs
│   └── Repository.cs
│
├── Interfaces/                  # Service interfaces
│   ├── IIncomeService.cs
│   ├── IBudgetService.cs
│   └── IAnalyticsService.cs
│
├── Middleware/                  # HTTP middleware
│   └── ExceptionHandlingMiddleware.cs
│
├── Extensions/                  # Extension methods & configurations
│   └── ServiceExtensions.cs
│
├── Utilities/                   # Helper utilities
│   ├── Constants.cs
│   ├── ValidationHelper.cs
│   ├── ApiResponse.cs
│   └── Extensions.cs
│
├── wwwroot/                     # Static assets
│   ├── css/
│   ├── js/
│   └── lib/
│
├── Program.cs                   # Application entry point & configuration
├── appsettings.json             # Application settings
└── SmartBudget.csproj          # Project file
```

## Architecture Overview

### 1. **Components Layer** (UI)
- Contains all Blazor components and pages
- Follows component-based architecture
- Separated into:
  - **Pages**: Routable components
  - **Layout**: Layout wrapper components
  - **Shared**: Reusable UI components

### 2. **Services Layer** (Business Logic)
- Contains business logic and application workflows
- Each service handles specific domain functionality
- Examples:
  - `IncomeService`: Manages income operations
  - `ExpenseService`: Manages expense operations
  - `BudgetService`: Manages budget operations
  - `AnalyticsService`: Provides financial analytics

### 3. **Models Layer** (Domain)
- Contains domain entities that map to database tables
- Represents core business concepts
- Examples: `Income`, `Expense`, `Budget`, `Category`

### 4. **DTOs Layer** (Data Transfer)
- Contains view models for data transfer between layers
- Prevents exposing internal models directly
- Examples: `IncomeDto`, `ExpenseDto`

### 5. **Data Access Layer**
- **ApplicationDbContext**: Entity Framework DbContext
- **IRepository/Repository**: Generic repository pattern for database operations
- **Migrations**: Database schema migration files

### 6. **Middleware Layer**
- Handles cross-cutting concerns
- `ExceptionHandlingMiddleware`: Global exception handling

### 7. **Utilities Layer**
- Helper methods and constants
- `ValidationHelper`: Input validation
- `Constants`: Application-wide constants
- `Extensions`: Extension methods for common operations
- `ApiResponse`: Standard API response wrappers

## Key Design Patterns

### Repository Pattern
```csharp
IRepository<T> - Generic interface for data access
Repository<T>  - Generic implementation for CRUD operations
```

### Service Pattern
```csharp
IIncomeService       - Contract for income operations
IncomeService        - Implementation of income operations
```

### DTO Pattern
Used to transfer data between layers while hiding internal models.

### Dependency Injection
Configured via `ServiceExtensions.cs`:
```csharp
services.AddApplicationServices();
```

## Shared Components

### AlertComponent
Displays alert messages (info, success, warning, danger)
```razor
<AlertComponent 
    AlertType="success" 
    Title="Success" 
    Message="Operation completed" />
```

### LoadingSpinner
Shows a loading indicator
```razor
<LoadingSpinner />
```

### EmptyStateComponent
Displays empty state messaging
```razor
<EmptyStateComponent Message="No data available" />
```

### TransactionFormComponent
Reusable form for transaction input
```razor
<TransactionFormComponent @bind-Model="formModel" OnSubmit="HandleSubmit" />
```

## Data Flow

1. **User Interaction** → Components/Pages
2. **Component** → Calls Service
3. **Service** → Uses Repository
4. **Repository** → Accesses Database via DbContext
5. **Database** → Returns data
6. **Repository** → Returns data to Service
7. **Service** → Applies business logic, returns to Component
8. **Component** → Updates View

## Adding New Features

### Steps to add a new entity (e.g., Savings Goal):

1. **Create Model** in `Models/SavingsGoal.cs`
2. **Create DTO** in `DTOs/SavingsGoalDto.cs`
3. **Create Service Interface** in `Interfaces/ISavingsGoalService.cs`
4. **Create Service** in `Services/SavingsGoalService.cs`
5. **Register in DI** - Update `Extensions/ServiceExtensions.cs`
6. **Create Migration** - `dotnet ef migrations add AddSavingsGoal`
7. **Create Components** in `Components/Pages/SavingsGoals.razor`

## Best Practices

- **Use Interfaces**: Always depend on abstractions, not concrete implementations
- **Separation of Concerns**: Keep business logic in services, UI in components
- **DTOs**: Use DTOs for API communication, not entities directly
- **Error Handling**: Use `ExceptionHandlingMiddleware` for consistent error responses
- **Validation**: Use `ValidationHelper` for input validation
- **Constants**: Use `Constants` class instead of magic strings/numbers

## Database

- **Technology**: SQL Server
- **ORM**: Entity Framework Core
- **Migrations**: Located in `Data/Migrations/`

### Running Migrations
```bash
dotnet ef database update
```

## Testing Notes

When testing services:
- Mock the repository dependencies
- Test business logic independently of database
- Use DTOs in service tests, not internal models

## Configuration

Application settings are configured in:
- `appsettings.json` - Shared settings
- `appsettings.Development.json` - Development-specific settings

Key configurations:
- Database connection string
- Authentication settings
- Logging levels
