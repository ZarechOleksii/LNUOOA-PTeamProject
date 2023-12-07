namespace SharedLib.Models
{
    public static class StandartCommandMessages
    {
        public const string Start = "Welcome to SafeVid bot! \nSend a link to a Tik-Tok or Instagram media you wish to download!";
        public const string Help = "Welcome to SafeVid bot! \nSend a link to a Tik-Tok or Instagram media you wish to download!";
        public const string TimedOut = "Timed out.";
        public const string UnknownError = "Sorry your request failed with unknown error, we will try to find the cause ASAP.";
        public const string Caption = "@TeleVideoSaverBot - [Source]({0})";
        public const string LinkNotSupported =
@"Sorry, it appears a link of this type is not supported.
Make sure tour link is pointing to one of these:
• Instagram Photo; 
• Instagram Reel;
• TikTok Video.";
    }
}
