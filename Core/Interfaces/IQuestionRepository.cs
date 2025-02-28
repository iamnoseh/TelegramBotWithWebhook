using TelegramBot.Core.Entities;

namespace Core.Interfaces;

public interface IQuestionRepository
{
    Task<List<Question>> GetAllQuestionsAsync();
    Task<Question> GetQuestionByIdAsync(int questionId);
}