using TelegramBot.Core.Entities;

namespace Core.Interfaces.TelegramBot.Core.Interfaces;

public interface IUserResponseRepository
{
    Task SaveResponseAsync(UserResponse response);
}