using Core.UseCases.TelegramBot.Core.UseCases;
using Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;
using Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Core.Entities;
using User = TelegramBot.Core.Entities.User;

namespace Infrastructure.Services.TelegramBot.Infrastructure.Services;

public class TelegramBotService(
    TelegramBotClient botClient,
    IMemoryCache cache,
    RegisterUser registerUser,
    GetRandomQuestion getRandomQuestion,
    SubmitAnswer submitAnswer,
    GetTopUsers getTopUsers,
    GetUserProfile getUserProfile)
{
    private readonly IMemoryCache _cache = cache;

    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Type == UpdateType.Message && update.Message != null)
        {
            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text;

            if (update.Message.Contact != null)
            {
                await HandleContactRegistration(chatId, update.Message.Contact);
                return;
            }

            switch (text)
            {
                case "/start":
                    await botClient.SendTextMessageAsync(chatId, "Хуш омадед!", replyMarkup: GetMainButtons());
                    break;
                case "Саволи нав":
                    await HandleNewQuestion(chatId);
                    break;
                case "Top":
                    await HandleTopCommand(chatId);
                    break;
                case "Profile":
                    await HandleProfileCommand(chatId);
                    break;
            }
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            await HandleCallbackQuery(update.CallbackQuery);
        }
    }

    private async Task HandleContactRegistration(long chatId, Contact contact)
    {
        var user = new User()
        {
            ChatId = chatId,
            PhoneNumber = contact.PhoneNumber,
            Name = contact.FirstName,
            Username = contact.FirstName,
            City = "Unknown",
            Score = 0
        };
        await registerUser.ExecuteAsync(user);
        await botClient.SendTextMessageAsync(chatId, "Сабт шудед!", replyMarkup: GetMainButtons());
    }

    private async Task HandleNewQuestion(long chatId)
    {
        var question = await getRandomQuestion.ExecuteAsync();
        if (question == null)
        {
            await botClient.SendTextMessageAsync(chatId, "Саволҳо мавҷуд нестанд.");
            return;
        }

        await botClient.SendTextMessageAsync(chatId,
            $"{question.QuestionText}\nA) {question.Option.FirstVariant}\nB) {question.Option.SecondVariant}\nC) {question.Option.ThirdVariant}\nD) {question.Option.FourthVariant}",
            replyMarkup: GetButtons(question.QuestionId));
    }

    private async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var messageId = callbackQuery.Message.MessageId;
        var data = callbackQuery.Data.Split('_');
        var questionId = int.Parse(data[0]);
        var selectedOption = data[1];

        var isCorrect = await submitAnswer.ExecuteAsync(chatId, questionId, selectedOption);
        var message = isCorrect ? "Офарин! +1 балл" : "Нодуруст!";
        await botClient.EditMessageTextAsync(chatId, messageId, message);
    }

    private IReplyMarkup GetMainButtons()
    {
        return new ReplyKeyboardMarkup
        {
            Keyboard = new List<List<KeyboardButton>>
            {
                new() { new KeyboardButton("Саволи нав"), new KeyboardButton("Top") },
                new() { new KeyboardButton("Profile"), new KeyboardButton("Help") }
            },
            ResizeKeyboard = true
        };
    }
    private IReplyMarkup GetButtons(int questionId)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("A", $"{questionId}_A"), InlineKeyboardButton.WithCallbackData("B", $"{questionId}_B") },
            new[] { InlineKeyboardButton.WithCallbackData("C", $"{questionId}_C"), InlineKeyboardButton.WithCallbackData("D", $"{questionId}_D") }
        });
    }

    private async Task HandleTopCommand(long chatId)
    {
        var topUsers = await getTopUsers.ExecuteAsync(50);
        var message = "Топ 50:\n";
        int rank = 1;
        foreach (var user in topUsers)
        {
            message += $"{rank++}. {user.Name} - {user.Score} балл\n";
        }
        await botClient.SendTextMessageAsync(chatId, message);
    }
    
    private async Task HandleProfileCommand(long chatId)
    {
        var profile = await getUserProfile.ExecuteAsync(chatId);
        if (profile == null)
        {
            await botClient.SendTextMessageAsync(chatId, "Шумо сабт нашудаед.");
            return;
        }
        await botClient.SendTextMessageAsync(chatId, $"Ном: {profile.Name}\nШаҳр: {profile.City}\nХолҳо: {profile.Score}");
    }
}