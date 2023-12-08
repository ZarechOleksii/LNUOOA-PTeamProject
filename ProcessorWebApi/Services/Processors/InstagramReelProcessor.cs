using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using ProcessorWebApi.Interfaces.Processors;
using ProcessorWebApi.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using SharedLib.Models;
using Telegram.Bot.Types.Enums;
using OpenQA.Selenium.Chrome;

namespace ProcessorWebApi.Services.Processors
{
    public class InstagramReelProcessor(ITelegramBotClient bot) : ProcessorBase(bot), IInstagramReelProcessor
    {
        private const string SrcAttribute = "src";
        private const string VideoXPath = "//article//video";

        public override async Task SendMediaAsync(GetMediaRequest request)
        {
            ChromeDriver driver = CreateDefaultDriver(request.MediaUri);

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            wait.Until(q => q.FindElements(By.XPath(VideoXPath)).Count == 1);
            var element = driver.FindElement(By.XPath(VideoXPath));
            string videoLink = element.GetAttribute(SrcAttribute);
            driver.Close();

            var client = new HttpClient();
            var responseStream = await client.GetStreamAsync(videoLink)!;

            await _bot.SendVideoAsync(
                request.ChatIdentifier,
                InputFile.FromStream(responseStream),
                caption: string.Format(StandartCommandMessages.Caption, request.MediaUri),
                parseMode: ParseMode.MarkdownV2);
        }
    }
}
