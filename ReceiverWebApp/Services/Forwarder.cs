using ReceiverWebApp.Services.Interfaces;

namespace ReceiverWebApp.Services
{
    public class Forwarder : IForwarder
    {
        const string ChatId = "ChatIdentifier";
        const string Link = "MediaUri";
        const string UriString = "http://20.105.0.2:3333/api/send-media";

        public async Task<HttpResponseMessage> ForwardAsync(long chatId, string link)
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

            return await client.SendAsync(request);
        }
    }
}
