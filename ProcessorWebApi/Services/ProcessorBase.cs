using ProcessorWebApi.Interfaces;
using ProcessorWebApi.Models;
using Telegram.Bot;

namespace ProcessorWebApi.Services
{
    public abstract class ProcessorBase(ITelegramBotClient bot) : IProcessor
    {
        protected readonly ITelegramBotClient _bot = bot;

        public abstract Task SendMediaAsync(GetMediaRequest request);
    }
}
