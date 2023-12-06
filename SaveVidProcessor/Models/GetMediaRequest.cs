using Telegram.Bot.Types;

namespace SaveVidProcessor.Models
{
    public class GetMediaRequest
    {
        public string MediaUri { get; set; }

        public long ChatIdentifier { get; set; }
    }
}
