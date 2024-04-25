using ProcessorWebApi.Interfaces;
using ProcessorWebApi.Interfaces.Processors;
using System.Text.RegularExpressions;

namespace ProcessorWebApi.Services
{
    public partial class ProcessorSelectorService : IProcessorSelectorService
    {
        private readonly IInstagramPhotoProcessor _instagramPhotoProcessor;
        private readonly IInstagramReelProcessor _instagramReelProcessor;
        private readonly ITikTokVideoProcessor _tikTokVideoProcessor;

        [GeneratedRegex(@"^(https?:\/\/)?(www\.)?instagram\.com\/p\/(.*)", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex InstagramPhotoRegex();

        [GeneratedRegex(@"^(https?:\/\/)?(www\.)?instagram\.com\/reels?\/(.*)", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex InstagramReelRegex();

        [GeneratedRegex(@"^(https?://)?(www\.)?tiktok\.com/@[\w\d-.]+/video/\d+", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex TikTokVideoPCRegex();

        [GeneratedRegex(@"^https://vm\.tiktok\.com/[\w-]+/$", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex TikTokVideoMobileRegex();

        public ProcessorSelectorService(
            IInstagramPhotoProcessor instagramPhotoProcessor,
            IInstagramReelProcessor instagramReelProcessor,
            ITikTokVideoProcessor tikTokVideoProcessor
            )
        {
            _instagramPhotoProcessor = instagramPhotoProcessor;
            _instagramReelProcessor = instagramReelProcessor;
            _tikTokVideoProcessor = tikTokVideoProcessor;
        }

        public bool IsInstagramPhotoUri(string uri) => InstagramPhotoRegex().IsMatch(uri);

        public bool IsInstagramReelUri(string uri) => InstagramReelRegex().IsMatch(uri);

        public bool IsTikTokVideoUri(string uri) => TikTokVideoPCRegex().IsMatch(uri) || TikTokVideoMobileRegex().IsMatch(uri);

        public IProcessor? GetRequiredProcessor(string uri) =>
            uri switch
            {
                _ when IsInstagramPhotoUri(uri) => _instagramPhotoProcessor,
                _ when IsInstagramReelUri(uri) => _instagramReelProcessor,
                _ when IsTikTokVideoUri(uri) => _tikTokVideoProcessor,
                _ => null,
            };
    }
}
