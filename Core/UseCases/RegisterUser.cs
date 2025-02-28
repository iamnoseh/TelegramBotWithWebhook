using Core.Interfaces.TelegramBot.Core.Interfaces;
using TelegramBot.Core.Entities;

namespace Core.UseCases.TelegramBot.Core.UseCases;

public class RegisterUser(IUserRepository userRepository)
{
    public async Task ExecuteAsync(User user)
    {
        await userRepository.AddUserAsync(user);
    }
}