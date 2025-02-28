using Infrastructure.Services.TelegramBot.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;


namespace TelegramBot.WebApi.Controllers;

[ApiController]
[Route("api/telegram")]
public class TelegramWebhookController(TelegramBotService botService) : ControllerBase
{
    [HttpPost("webhook")]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await botService.HandleUpdateAsync(update);
        return Ok();
    }
}