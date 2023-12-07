using ProcessorWebApi.Interfaces.Processors;
using ProcessorWebApi.Models;
using Telegram.Bot;

namespace ProcessorWebApi.Services.Processors
{
    public class TikTokVideoProcessor(ITelegramBotClient bot) : ProcessorBase(bot), ITikTokVideoProcessor
    {
        public override async Task SendMediaAsync(GetMediaRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
