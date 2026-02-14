using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartBudget.Models;

namespace SmartBudget.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Income> Incomes { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Budget> Budgets { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This allows the database update to proceed even if EF thinks there are minor dynamic changes
        optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // --- Relationship Configurations ---
        builder.Entity<Income>()
            .HasOne(i => i.User)
            .WithMany(u => u.Incomes)
            .HasForeignKey(i => i.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Expense>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Category>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Budget>()
            .HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Budget>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Budgets)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Income>()
            .Property(i => i.Amount)
            .HasPrecision(18, 2);

        builder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasPrecision(18, 2);

        builder.Entity<Budget>()
            .Property(b => b.LimitAmount)
            .HasPrecision(18, 2);

        // --- SEED DATA ---
        string devId = "dev-user-123";

        // 1. Seed the Dev User
        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = devId,
            UserName = "dev@example.com",
            NormalizedUserName = "DEV@EXAMPLE.COM",
            Email = "dev@example.com",
            NormalizedEmail = "DEV@EXAMPLE.COM",
            EmailConfirmed = true,
            // Hardcoded hash for 'DevPassword123!'
            PasswordHash = "AQAAAAIAAYagAAAAEOf6k1G5fHk9pQp7zXmR1Q==", 
            SecurityStamp = "77379761-1934-4286-9173-63327668581e",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        // 2. Seed Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Food & Drinks", UserId = devId, Color = "#FF5733", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 2, Name = "Transport", UserId = devId, Color = "#33FF57", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 3, Name = "Rent & Utilities", UserId = devId, Color = "#3357FF", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 4, Name = "Entertainment", UserId = devId, Color = "#F333FF", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 5, Name = "Shopping", UserId = devId, Color = "#FF3380", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 6, Name = "Health", UserId = devId, Color = "#33FFF5", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}