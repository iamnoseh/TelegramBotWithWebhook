using System.Net;
using System.Net.Sockets;
using Core.Interfaces;
using Core.Interfaces.TelegramBot.Core.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.TelegramBot.Infrastructure.Data.Repositories;
using Infrastructure.Data.TelegramBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Гирифтани IP-и сервер
var host = Dns.GetHostName();
var ip = Dns.GetHostAddresses(host)
    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?.ToString();

if (string.IsNullOrEmpty(ip))
{
    Console.WriteLine("⚠️ IP-и сервер ёфт нашуд!");
    return;
}

Console.WriteLine($"✅ Сервер дар IP: {ip} кор мекунад");

// Танзими Kestrel барои HTTPS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 80); // HTTP
    serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.UseHttps("/etc/letsencrypt/live/yourdomain.com/fullchain.pem",
            "/etc/letsencrypt/live/yourdomain.com/privkey.pem");  // SSL
    });
});

// Пайвастшавӣ ба PostgreSQL
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Бақайдгирии хидматҳо
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

// Танзими Telegram Bot
var botToken = builder.Configuration["BotConfiguration:Token"];
if (string.IsNullOrEmpty(botToken))
{
    throw new Exception("⚠️ Token-и TelegramBot холӣ аст!");
}

builder.Services.AddSingleton<TelegramBotClient>(sp => new TelegramBotClient(botToken));

var app = builder.Build();

// Танзими Webhook бо порти 443
var botClient = app.Services.GetRequiredService<TelegramBotClient>();
var webhookUrl = $"https://{ip}/api/telegram/webhook";
await botClient.SetWebhookAsync(webhookUrl);

Console.WriteLine($"✅ Webhook танзим шуд: {webhookUrl}");

app.MapControllers();
app.Run();