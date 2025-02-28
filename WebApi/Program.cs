using Core.Interfaces;
using Core.Interfaces.TelegramBot.Core.Interfaces;
using Core.UseCases.TelegramBot.Core.UseCases;
using Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;
using Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases.TelegramBot.Core.UseCases;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.TelegramBot.Infrastructure.Data.Repositories;
using Infrastructure.Data.TelegramBot.Infrastructure.Data;
using Infrastructure.Services.TelegramBot.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IOptionRepository, OptionRepository>();
builder.Services.AddScoped<IUserResponseRepository, UserResponseRepository>();

builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<GetRandomQuestion>();
builder.Services.AddScoped<SubmitAnswer>();
builder.Services.AddScoped<GetTopUsers>();
builder.Services.AddScoped<GetUserProfile>();

builder.Services.AddSingleton<TelegramBotClient>(sp =>
    new TelegramBotClient(builder.Configuration["BotConfiguration:Token"]));
builder.Services.AddScoped<TelegramBotService>();

builder.Services.AddMemoryCache();
builder.Services.AddControllers();

var app = builder.Build();

// Танзими Webhook
var botClient = app.Services.GetRequiredService<TelegramBotClient>();
var webhookUrl = "https://thin-guests-search.loca.lt/api/telegram/webhook";
await botClient.SetWebhookAsync(webhookUrl);

app.MapControllers();
app.Urls.Add("http://31.130.144.99:5210");
app.Run();