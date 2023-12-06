using SaveVidReceiver.Models;
using SaveVidReceiver.Extensions;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new Exception("No bot config.");
var botToken = "BOT_TOKEN";

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
