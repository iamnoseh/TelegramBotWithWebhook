using Core.Interfaces.TelegramBot.Core.Interfaces;
using Infrastructure.Data.TelegramBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Core.Entities;

namespace Infrastructure.Data.Repositories.TelegramBot.Infrastructure.Data.Repositories;

public class OptionRepository(DataContext context) : IOptionRepository
{
    public async Task<Option> GetOptionByIdAsync(int optionId)
    {
        return await context.Options.FirstOrDefaultAsync(o => o.OptionId == optionId);
    }
}