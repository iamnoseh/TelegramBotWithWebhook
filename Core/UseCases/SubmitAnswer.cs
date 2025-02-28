using Core.Interfaces;
using Core.Interfaces.TelegramBot.Core.Interfaces;
using TelegramBot.Core.Entities;

namespace Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;

public class SubmitAnswer(
    IUserResponseRepository responseRepository,
    IQuestionRepository questionRepository,
    IUserRepository userRepository)
{
    public async Task<bool> ExecuteAsync(long chatId, int questionId, string selectedOption)
    {
        var question = await questionRepository.GetQuestionByIdAsync(questionId);
        if (question == null) return false;

        bool isCorrect = selectedOption.Trim().ToUpper() switch
        {
            "A" => question.Option.Answer == question.Option.FirstVariant,
            "B" => question.Option.Answer == question.Option.SecondVariant,
            "C" => question.Option.Answer == question.Option.ThirdVariant,
            "D" => question.Option.Answer == question.Option.FourthVariant,
            _ => false
        };

        var response = new UserResponse
        {
            ChatId = chatId,
            QuestionId = questionId,
            SelectedOption = selectedOption,
            IsCorrect = isCorrect
        };
        await responseRepository.SaveResponseAsync(response);

        if (isCorrect)
        {
            var user = await userRepository.GetUserByChatIdAsync(chatId);
            user.Score += 1;
            await userRepository.UpdateUserAsync(user);
        }

        return isCorrect;
    }
}