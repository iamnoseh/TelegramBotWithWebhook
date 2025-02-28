using Core.Interfaces.TelegramBot.Core.Interfaces;
using Infrastructure.Data.TelegramBot.Infrastructure.Data;
using TelegramBot.Core.Entities;

namespace Infrastructure.Data.Repositories;

public class UserResponseRepository(DataContext context) : IUserResponseRepository
{
    public async Task SaveResponseAsync(UserResponse response)
    {
        await context.UserResponses.AddAsync(response);
        await context.SaveChangesAsync();
    }
}