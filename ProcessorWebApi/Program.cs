using Azure.Identity;
using ProcessorWebApi.Interfaces;
using ProcessorWebApi.Interfaces.Processors;
using ProcessorWebApi.Services;
using ProcessorWebApi.Services.Processors;
using SharedLib.Extensions;
using SharedLib.Models;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new Exception("No bot config.");

string botToken;

Uri keyVaultURL = new(builder.Configuration.GetSection("KeyVaultURL").Value!);
var azureCredential = new DefaultAzureCredential();
builder.Configuration.AddAzureKeyVault(keyVaultURL, azureCredential);
botToken = builder.Configuration.GetSection("BotToken").Value!;

botConfiguration.BotToken = botToken;
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

// Add services to the container.
builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    botConfig.BotToken = botToken;
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

builder.Services.AddSingleton<IProcessorSelectorService, ProcessorSelectorService>();
builder.Services.AddSingleton<IInstagramPhotoProcessor, InstagramPhotoProcessor>();
builder.Services.AddSingleton<IInstagramReelProcessor, InstagramReelProcessor>();
builder.Services.AddSingleton<ITikTokVideoProcessor, TikTokVideoProcessor>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
