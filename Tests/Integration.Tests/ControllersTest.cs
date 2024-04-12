namespace Integration.Tests
{
    public class ControllersTest
    {
        private readonly HttpClient _httpClient;

        public ControllersTest()
        {
            _httpClient = new HttpClient();
        }

        [TestCase("www.google.com")]
        [TestCase("random string")]
        [TestCase("123@gmail.com")]
        [TestCase("https://gmail.com")]
        [TestCase("http://not_a_virus.com")]
        [TestCase("C:/ProgramFiles")]
        public async Task SendMedia_InvalidRequestUrl_ReturnsBadRequest(string invalidMediaUrl)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "ChatIdentifier", "12345" },
                { "MediaUri", invalidMediaUrl }
            });

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/bot"),
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            Assert.That(response.IsSuccessStatusCode, Is.False);
        }

        [TestCase("11111111111111111111111111111111")]
        [TestCase("asdasdsad12242345234543535")]
        [TestCase("definitely not valid id")]
        [TestCase("12378632458768546819234568126235182673418296312")] // is type LONG but too long for tg id
        public async Task SendMedia_InvalidRequestChatId_ReturnsBadRequest(string invalidChatId)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "ChatIdentifier", invalidChatId },
                { "MediaUri", "https://www.instagram.com/p/Cki1JCsJK_V/" }
            });

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://20.105.0.2:3333/api/send-media"),
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
