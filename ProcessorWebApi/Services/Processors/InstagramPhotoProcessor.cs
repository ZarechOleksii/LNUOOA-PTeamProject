using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ProcessorWebApi.Interfaces.Processors;
using ProcessorWebApi.Models;
using SharedLib.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProcessorWebApi.Services.Processors
{
    public class InstagramPhotoProcessor(ITelegramBotClient bot) : ProcessorBase(bot), IInstagramPhotoProcessor
    {
        private const string SrcAttribute = "src";
        private const string ImageXPath = "//article//div[contains(@style, 'padding-bottom: 100%;')]//img";

        public override async Task SendMediaAsync(GetMediaRequest request)
        {
            ChromeDriver driver = CreateDefaultDriver(request.MediaUri);

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            wait.Until(q => q.FindElements(By.XPath(ImageXPath)).Count == 1);
            var element = driver.FindElement(By.XPath(ImageXPath));
            string imgLink = element.GetAttribute(SrcAttribute);
            driver.Close();

            var client = new HttpClient();
            var responseStream = await client.GetStreamAsync(imgLink)!;

            await _bot.SendPhotoAsync(
                request.ChatIdentifier, 
                InputFile.FromStream(responseStream), 
                caption: string.Format(StandartCommandMessages.Caption, request.MediaUri), 
                parseMode: ParseMode.MarkdownV2);
        }
    }
}
