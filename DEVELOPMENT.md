# Smart Budget - Development Setup Guide

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

- **.NET 10 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **SQL Server LocalDB** - Included with Visual Studio or [download separately](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- **Visual Studio Code** or **Visual Studio 2022**
- **Git** - For version control

## Project Setup

### 1. Clone the Repository
```bash
git clone https://github.com/Basseychrist/CSE-325-Team-13-Dotnet-Project.git
cd CSE-325-Team-13-Dotnet-Project
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Configure the Database

The project uses SQL Server LocalDB by default. The connection string is configured in `appsettings.json`:

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartBudgetDb;Trusted_Connection=true;"
```

### 4. Apply Database Migrations

Run the following command to create the database schema:

```bash
dotnet ef database update
```

This will:
- Create the `SmartBudgetDb` database in LocalDB
- Create all tables (Users, Incomes, Expenses, Categories, Budgets)
- Apply default constraints and relationships

### 5. Build the Project

```bash
dotnet build
```

## Running the Application

### Development Mode
```bash
dotnet run
```

The application will be available at `https://localhost:7139` (or similar)

### Hot Reload
To enable hot reload during development:
```bash
dotnet watch run
```

## Project Structure

```
SmartBudget/
├── Components/          # Blazor components and pages
├── Models/             # Data models (User, Income, Expense, Category, Budget)
├── Data/               # Database context and configuration
├── Repositories/       # Generic repository pattern implementation
├── Services/           # Business logic services
├── DTOs/              # Data transfer objects for API operations
├── Migrations/        # Entity Framework Core migrations
├── wwwroot/           # Static files (CSS, JavaScript, images)
└── Program.cs         # Application startup configuration
```

## Database Schema

### Tables Created:

1. **AspNetUsers** - User account information (extends Identity tables)
2. **Incomes** - User income records
3. **Expenses** - User expense records
4. **Categories** - Expense categories
5. **Budgets** - Monthly budget limits per category

## Key Technologies

- **Framework**: ASP.NET Core (Blazor Web App)
- **Language**: C#
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity

## Architecture

The project follows a layered architecture:

- **Components Layer**: Blazor components for UI
- **Service Layer**: Business logic and data operations
- **Repository Layer**: Data access abstraction
- **Data Layer**: Entity Framework Core DbContext
- **Models**: Domain entities

## Common Commands

### Create a New Migration
```bash
dotnet ef migrations add <MigrationName>
```

### Remove the Latest Migration (if not applied to DB)
```bash
dotnet ef migrations remove
```

### View Database Migrations History
```bash
dotnet ef migrations list
```

### Generate Database Script
```bash
dotnet ef migrations script > migration.sql
```

## Development Workflow

1. **Create a new branch** for your feature
2. **Make changes** to the code
3. **Test locally** with `dotnet run`
4. **Run migrations** if you update the data model
5. **Commit and push** your changes
6. **Create a pull request** on GitHub

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server LocalDB is running
- Check the connection string in `appsettings.json`
- Verify SQL Server instance: `sqllocaldb info`

### Migration Errors
- Delete `Migrations` folder and `.mdf` database file to start fresh
- Run: `dotnet ef database update`

### Build Errors
- Run: `dotnet clean && dotnet build`
- Delete `obj` and `bin` folders manually if needed

### Port Already in Use
The app runs on localhost with HTTPS. If you get a port conflict error, change the port in `Properties/launchSettings.json`

## Team Resources

- **GitHub Repository**: [CSE-325-Team-13-Dotnet-Project](https://github.com/Basseychrist/CSE-325-Team-13-Dotnet-Project)
- **Trello Board**: [CSE-325 Blazor App Project](https://trello.com/b/BU4WeMet/cse-325-blazor-app-project)
- **Documentation**: See [README.md](../README.md) for project overview
