using Core.Interfaces;
using Infrastructure.Data.TelegramBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Core.Entities;

namespace Infrastructure.Data.Repositories;

public class QuestionRepository(DataContext context) : IQuestionRepository
{
    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await context.Questions.Include(q => q.Option).ToListAsync();
    }

    public async Task<Question> GetQuestionByIdAsync(int questionId)
    {
        return await context.Questions.Include(q => q.Option)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);
    }
}