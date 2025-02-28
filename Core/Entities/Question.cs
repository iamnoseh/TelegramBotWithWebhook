namespace TelegramBot.Core.Entities;

public class Question
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public int OptionId { get; set; }
    public Option Option { get; set; }
}