using Microsoft.EntityFrameworkCore;
using TelegramBot.Core.Entities;

namespace Infrastructure.Data.TelegramBot.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<UserResponse> UserResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>()
            .HasOne(o => o.Question)
            .WithOne(q => q.Option)
            .HasForeignKey<Question>(q => q.OptionId);
    }
}