using Microsoft.AspNetCore.Mvc;
using ProcessorWebApi.Interfaces;
using ProcessorWebApi.Models;
using SharedLib.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProcessorWebApi.Controllers
{
    [ApiController]
    public class HomeController(ITelegramBotClient bot, IProcessorSelectorService processorSelectorService, ILogger<HomeController> logger) : Controller
    {
        private static readonly CancellationTokenSource _cts = new();

        [HttpPost("api/send-media")]
        public async Task<IActionResult> SendMedia([FromForm] GetMediaRequest request)
        {
            logger.LogDebug("Received a request from: {ChatIdentifier}; uri: {MediaUri}.", request.ChatIdentifier, request.MediaUri);

            IProcessor? processor = processorSelectorService.GetRequiredProcessor(request.MediaUri);

            if (processor is null)
            {
                logger.LogDebug("{MediaUri} is not currently supported.", request.MediaUri);
                await bot.SendTextMessageAsync(new ChatId(request.ChatIdentifier), StandartCommandMessages.LinkNotSupported);
                return BadRequest("{MediaUri} is not currently supported.");
            }

            logger.LogDebug("{MediaUri} is handled by: {ProcessorType}", request.MediaUri, processor.GetType());
            try
            {
                _cts.CancelAfter(TimeSpan.FromSeconds(60));
                await processor.SendMediaAsync(request);
                return Ok();
            }
            catch (OperationCanceledException)
            {
                logger.LogError("{MediaUri} was timed out.", request.MediaUri);
                await bot.SendTextMessageAsync(new ChatId(request.ChatIdentifier), StandartCommandMessages.TimedOut);
                return BadRequest("xyi1");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{MediaUri} failed with exception.", request.MediaUri);
                await bot.SendTextMessageAsync(new ChatId(request.ChatIdentifier), StandartCommandMessages.UnknownError);
                return BadRequest("xyi2");
            }
        }
    }
}
