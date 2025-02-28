using TelegramBot.Core.Entities;

namespace Core.Interfaces.TelegramBot.Core.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User> GetUserByChatIdAsync(long chatId);
    Task UpdateUserAsync(User user);
    Task<List<User>> GetTopUsersAsync(int limit);
}