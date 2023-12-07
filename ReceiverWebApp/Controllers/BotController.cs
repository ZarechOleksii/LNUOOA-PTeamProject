using Microsoft.AspNetCore.Mvc;
using ReceiverWebApp.Filters;
using ReceiverWebApp.Services;
using Telegram.Bot.Types;

namespace ReceiverWebApp.Controllers
{
    public class BotController : Controller
    {
        [HttpPost]
        [ValidateTelegramBot]
        public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
        {
            await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
            return Ok();
        }
    }
}
