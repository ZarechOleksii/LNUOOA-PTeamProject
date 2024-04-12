using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Telegram.Bot;

namespace ProcessorWebApi.Tests
{
    public class IntegrationTests
    {
        private readonly HttpClient _httpClient;

        public IntegrationTests()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.Single(d => d.ServiceType == typeof(ITelegramBotClient));
                        services.Remove(descriptor);

                        descriptor = services.Single(d => d.ServiceType == typeof(HttpClient));
                        services.Remove(descriptor);

                        services.AddSingleton(new Mock<ITelegramBotClient>().Object);
                    });
                });
            _httpClient = application.CreateClient();
        }

        [TestCase("www.google.com")]
        [TestCase("random string")]
        [TestCase("123@gmail.com")]
        [TestCase("https://gmail.com")]
        [TestCase("http://not_a_virus.com")]
        [TestCase("C:/ProgramFiles")]
        public async Task SendMedia_InvalidRequestUrl_ReturnsBadRequest(string invalidMediaUrl)
        {
            //arrange
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "ChatIdentifier", "1111111" },
                { "MediaUri", invalidMediaUrl }
            });

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/send-media", UriKind.Relative),
                Content = content
            };

            //act
            var response = await _httpClient.SendAsync(request);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccessStatusCode, Is.False);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
