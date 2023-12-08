using OpenQA.Selenium.Chrome;
using ProcessorWebApi.Interfaces;
using ProcessorWebApi.Models;
using Telegram.Bot;

namespace ProcessorWebApi.Services
{
    public abstract class ProcessorBase(ITelegramBotClient bot) : IProcessor
    {
        protected readonly ITelegramBotClient _bot = bot;

        protected ChromeDriver CreateDefaultDriver(string uri)
        {
            ChromeOptions options = new();
            options.AddArgument("--headless");

            return new ChromeDriver(options)
            {
                Url = uri
            };
        }

        public abstract Task SendMediaAsync(GetMediaRequest request);
    }
}
