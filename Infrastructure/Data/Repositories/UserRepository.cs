using Core.Interfaces.TelegramBot.Core.Interfaces;
using Infrastructure.Data.TelegramBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Core.Entities;

namespace Infrastructure.Data.Repositories.TelegramBot.Infrastructure.Data.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<User> GetUserByChatIdAsync(long chatId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task<List<User>> GetTopUsersAsync(int limit)
    {
        return await context.Users.OrderByDescending(u => u.Score).Take(limit).ToListAsync();
    }
}