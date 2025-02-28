using Core.Interfaces.TelegramBot.Core.Interfaces;
using TelegramBot.Core.Entities;

namespace Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;

public class GetTopUsers(IUserRepository userRepository)
{
    public async Task<List<User>> ExecuteAsync(int limit)
    {
        return await userRepository.GetTopUsersAsync(limit);
    }
}