using Microsoft.Extensions.Logging.ApplicationInsights;
using Telegram.Bot;
using SaveVidReceiver.Controllers;
using SaveVidReceiver.Extensions;
using SaveVidReceiver.Models;
using SaveVidReceiver.Services;
using SaveVidReceiver.Services.Interfaces;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);

Uri keyVaultURL = new (builder.Configuration.GetSection("KeyVaultURL").Value!);
var azureCredential = new DefaultAzureCredential();
builder.Configuration.AddAzureKeyVault(keyVaultURL, azureCredential);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new Exception("No bot config.");
var botToken = builder.Configuration.GetSection("BotToken").Value!;

botConfiguration.BotToken = botToken;
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

// Add services to the container.
builder.Services.AddSingleton<IForwarder, Forwarder>();
builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    botConfig.BotToken = botToken;
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });
builder.Services.AddScoped<UpdateHandlers>();
builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("category", LogLevel.Trace);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
