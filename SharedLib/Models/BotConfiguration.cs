namespace SharedLib.Models
{
    public class BotConfiguration
    {
        public static readonly string Configuration = "BotConfiguration";

        public string BotToken { get; set; } = default!;
        public string HostAddress { get; init; } = default!;
        public string Route { get; init; } = default!;
        public string SecretToken { get; set; } = default!;
    }
}
