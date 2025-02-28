using Core.Interfaces.TelegramBot.Core.Interfaces;
using TelegramBot.Core.Entities;

namespace Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;

public class GetUserProfile(IUserRepository userRepository)
{
    public async Task<User> ExecuteAsync(long chatId)
    {
        return await userRepository.GetUserByChatIdAsync(chatId);
    }
}