using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartBudget.Components;
using SmartBudget.Data;
using SmartBudget.Models;
using SmartBudget.Repositories;
// ADD THESE TWO LINES:
using SmartBudget.Interfaces;
using SmartBudget.Services;
//account
using Microsoft.AspNetCore.Components.Authorization;
using SmartBudget.Components.Account;

using Microsoft.EntityFrameworkCore.Diagnostics;
// OS DETECTION (MACOS/WINDOWS):
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add repository services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


// Register IdentityRedirectManager for DI
builder.Services.AddScoped<SmartBudget.Components.Account.IdentityRedirectManager>();

// REGISTER YOUR EXPENSE SERVICE HERE:
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();


app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.MapAdditionalIdentityEndpoints();
app.Run();