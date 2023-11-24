using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot;
using SaveVidReceiver.Services.Interfaces;
using SaveVidReceiver.Models;

namespace SaveVidReceiver.Services
{
    public class UpdateHandlers
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandlers> _logger;
        private readonly IForwarder _forwarder;

        public UpdateHandlers(ITelegramBotClient botClient, ILogger<UpdateHandlers> logger, IForwarder forwarder)
        {
            _botClient = botClient;
            _logger = logger;
            _forwarder = forwarder;
        }

        public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: var message } when message is not null => BotOnMessageReceived(message, cancellationToken),
                _ => UnknownUpdateHandlerAsync()
            };

            await handler;
        }

        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            string messageText = message.Text ?? "error";

            string? commandResults = messageText.Split(' ')?[0] switch
            {
                "/start" => StandartCommandMessages.Start,
                "/help" => StandartCommandMessages.Help,
                _ => null
            };

            if(commandResults is not null && message is not null)
            {
                var sentMessage = await _botClient.SendTextMessageAsync(
                    message.Chat.Id, 
                    commandResults, 
                    replyToMessageId: message.ReplyToMessage?.MessageId, 
                    cancellationToken: cancellationToken);
                _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
                return;
            }

            if (IsValidLink(messageText) && message is not null)
            {
                await _forwarder.ForwardAsync(message.Chat.Id, messageText);
            }
        }

        public static bool IsValidLink(string? message)
        {
            if (Uri.IsWellFormedUriString(message, UriKind.Absolute))
            {
                return true;
            }

            return false;
        }

        private Task UnknownUpdateHandlerAsync()
        {
            return Task.CompletedTask;
        }
    }
}
