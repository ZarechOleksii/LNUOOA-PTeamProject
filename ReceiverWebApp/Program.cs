using Azure.Identity;
using ReceiverWebApp.Controllers;
using ReceiverWebApp.Extensions;
using ReceiverWebApp.Services;
using ReceiverWebApp.Services.Interfaces;
using SharedLib.Extensions;
using SharedLib.Models;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new Exception("No bot config.");

Uri keyVaultURL = new(builder.Configuration.GetSection("KeyVaultURL").Value!);

if (builder.Environment.IsProduction())
{
    var azureCredential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultURL, azureCredential);
}

string botToken = builder.Configuration.GetSection("BotToken").Value!;

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
