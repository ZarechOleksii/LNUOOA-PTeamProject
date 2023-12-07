using ReceiverWebApp.Services.Interfaces;

namespace ReceiverWebApp.Services
{
    public class Forwarder : IForwarder
    {
        const string ChatId = "ChatId";
        const string Link = "Link";
        const string UriString = "";

        public async Task ForwardAsync(long chatId, string link)
        {
            HttpClient client = new();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { ChatId, chatId.ToString() },
                { Link, link }
            });

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(UriString),
                Content = content
            };

            await client.SendAsync(request);
        }
    }
}
