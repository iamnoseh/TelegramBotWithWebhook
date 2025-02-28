using TelegramBot.Core.Entities;

namespace Core.Interfaces.TelegramBot.Core.Interfaces;

public interface IOptionRepository
{
    Task<Option> GetOptionByIdAsync(int optionId);
}