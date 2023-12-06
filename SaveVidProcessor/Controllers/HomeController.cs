using Microsoft.AspNetCore.Mvc;
using SaveVidProcessor.Models;
using SaveVidProcessor.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace SaveVidProcessor.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ITelegramBotClient Bot;

        public HomeController(ITelegramBotClient bot)
        {
            Bot = bot;
        }

        [HttpPost("api/get-media")]
        public async Task GetMedia([FromForm] GetMediaRequest request, CancellationToken cancellationToken)
        {
            if (!LinkVerificationService.IsValidUrl(request.MediaUri))
            {
                await Bot.SendTextMessageAsync(new ChatId(request.ChatIdentifier), "not a meme :(", cancellationToken: cancellationToken);
            }
            else
            {
                await Bot.SendTextMessageAsync(new ChatId(request.ChatIdentifier), "good meme, funny :)", cancellationToken: cancellationToken);
            }
        }
    }
}
