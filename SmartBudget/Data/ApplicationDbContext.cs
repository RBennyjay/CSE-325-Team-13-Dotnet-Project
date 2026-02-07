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

        // 1. Seed the Dev User so the Foreign Key exists
        var hasher = new PasswordHasher<ApplicationUser>();
        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = devId,
            UserName = "dev@example.com",
            NormalizedUserName = "DEV@EXAMPLE.COM",
            Email = "dev@example.com",
            NormalizedEmail = "DEV@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null!, "DevPassword123!"),
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        });

        // 2. Seed Categories (Linked to devId)
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Food & Drinks", UserId = devId, Color = "#FF5733", CreatedAt = DateTime.UtcNow },
            new Category { Id = 2, Name = "Transport", UserId = devId, Color = "#33FF57", CreatedAt = DateTime.UtcNow },
            new Category { Id = 3, Name = "Rent & Utilities", UserId = devId, Color = "#3357FF", CreatedAt = DateTime.UtcNow },
            new Category { Id = 4, Name = "Entertainment", UserId = devId, Color = "#F333FF", CreatedAt = DateTime.UtcNow },
            new Category { Id = 5, Name = "Shopping", UserId = devId, Color = "#FF3380", CreatedAt = DateTime.UtcNow },
            new Category { Id = 6, Name = "Health", UserId = devId, Color = "#33FFF5", CreatedAt = DateTime.UtcNow }
        );
    }
}