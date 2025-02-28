using Core.Interfaces;
using TelegramBot.Core.Entities;

namespace Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;

public class GetRandomQuestion(IQuestionRepository questionRepository)
{
    public async Task<Question> ExecuteAsync()
    {
        var questions = await questionRepository.GetAllQuestionsAsync();
        if (!questions.Any()) return null;
        var random = new Random();
        return questions[random.Next(questions.Count)];
    }
}
