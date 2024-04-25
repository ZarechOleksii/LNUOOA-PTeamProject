using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Integration.Tests
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

        [Test]
        public async Task SendMedia_NoHeaderFromWebhook_ReturnsUnauthorized()
        {
            //arrange
            var content = new StringContent(
                JsonSerializer.Serialize(new Update()), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/bot", UriKind.Relative),
                Content = content
            };

            //act
            var response = await _httpClient.SendAsync(request);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccessStatusCode, Is.False);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }

        [Test]
        public async Task SendMedia_AllIsWell_ReturnsSuccess()
        {
            //arrange
            var content = new StringContent(
                JsonSerializer.Serialize(new Update()), Encoding.UTF8, "application/json");

            content.Headers.Add("X-Telegram-Bot-Api-Secret-Token", "extemlySecretTokenKekes");

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/bot", UriKind.Relative),
                Content = content
            };

            //act
            var response = await _httpClient.SendAsync(request);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccessStatusCode, Is.True);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
