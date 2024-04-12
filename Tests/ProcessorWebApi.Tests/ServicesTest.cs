using ProcessorWebApi.Interfaces.Processors;
using ProcessorWebApi.Services;
using ProcessorWebApi.Services.Processors;
using Telegram.Bot;

namespace ProcessorWebApi.Tests
{
    public class ServicesTest
    {
        private ProcessorSelectorService _processorSelectorService = new (
            new InstagramPhotoProcessor(null), 
            new InstagramReelProcessor(null), 
            new TikTokVideoProcessor(null));

        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase("https://www.instagram.com/p/C2DR08BNHzl/")]
        [TestCase("https://www.instagram.com/p/CrP5vAChFv6/")]
        [TestCase("https://www.instagram.com/p/CncdzLzhQ3Z/")]
        [TestCase("https://www.instagram.com/p/CnaRaLGh9xl/")]
        [TestCase("https://www.instagram.com/p/Cki1JCsJK_V/")]
        public void IsValidInstagramPhotoLinkTest(string link)
        {
            Assert.That(_processorSelectorService.IsInstagramPhotoUri(link), Is.True);
        }

        [TestCase("htt://www.instagram.com/p/C2DR08BNHzl/")]
        [TestCase("i am not a link")]
        [TestCase("gaining consciousness")]
        public void IsValidInstagramPhotoUriFailsForInvalidUriTest(string link)
        {
            Assert.That(_processorSelectorService.IsInstagramPhotoUri(link), Is.False);
        }

        [TestCase("https://www.instagram.com/reel/C5G95NexSg3/?igsh=MWkzM2pmd3lvaTRwcQ==")]
        [TestCase("https://www.instagram.com/reel/C3-bgcUsyFH/?igsh=NnJsNm13eXdvbmUx")]
        [TestCase("https://www.instagram.com/reels/C4-V2WGtqZI/")]
        [TestCase("https://www.instagram.com/reels/C5WYNvhvkp2/")]
        public void IsValidInstagramReelLinkTest(string link)
        {
            Assert.That(_processorSelectorService.IsInstagramReelUri(link), Is.True);
        }

        [TestCase("htps://www.instagram.com/reel/C5G95NexSg3/?igsh=MWkzM2pmd3lvaTRwcQ==")]
        [TestCase("https://www.google.com/reel/C3-bgcUsyFH/?igsh=NnJsNm13eXdvbmUx")]
        [TestCase("still not a link")]
        [TestCase("c:/users/admin/i_am_not_getting_paid_for_this")]
        public void IsValidInstagramReelFailsForInvalidUriTest(string link)
        {
            Assert.That(_processorSelectorService.IsInstagramReelUri(link), Is.False);
        }

        [TestCase("https://vm.tiktok.com/ZMMmgfHeB/")]
        [TestCase("https://vm.tiktok.com/asdasda/")]
        [TestCase("https://vm.tiktok.com/dfh/")]
        [TestCase("https://vm.tiktok.com/ZMMjghjkghjghjmgfHeB/")]
        public void IsValidTikTokFromMobileLinkTest(string link)
        {
            Assert.That(_processorSelectorService.IsTikTokVideoUri(link), Is.True);
        }

        [TestCase("https://vm.petrocich.com/ZMMmgfHeB/")]
        [TestCase("consciousness gained")]
        [TestCase("htps://vm.tiktok.com/dfh/")]
        [TestCase("http://vm.tiktok.com/dfh/")]
        public void IsValidTikTokFromMobileFailsForInvalidUriTest(string link)
        {
            Assert.That(_processorSelectorService.IsTikTokVideoUri(link), Is.False);
        }

        [TestCase("https://www.tiktok.com/@teiepni/video/7345511492961324293")]
        [TestCase("https://www.tiktok.com/@gavryliv.andriy/video/7346528354222296325")]
        [TestCase("https://www.tiktok.com/@film_flow/video/7338732901816700166")]
        [TestCase("https://www.tiktok.com/@dyadichkin/video/7351005582641548549")]
        public void IsValidTikTokFromPCLinkTest(string link)
        {
            Assert.That(_processorSelectorService.IsTikTokVideoUri(link), Is.True);
        }

        [TestCase("htts://www.tiktok.com/@teiepni/video/7345511492961324293")]
        [TestCase("https://www.instagram.com/reels/C5WYNvhvkp2/")]
        [TestCase("http://www.tiktok.com/@dyadichkin/video/7351005582641548549")]
        [TestCase("kill all humans")]
        public void IsValidTikTokFromPCFailsForInvalidLinkTest(string link)
        {
            Assert.That(_processorSelectorService.IsTikTokVideoUri(link), Is.False);
        }
    }
}