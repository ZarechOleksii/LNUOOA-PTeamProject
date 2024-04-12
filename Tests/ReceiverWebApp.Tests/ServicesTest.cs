using ReceiverWebApp.Services;

namespace ReceiverWebApp.Tests
{
    public class ServicesTest
    {
        [TestCase("https://www.google.com")]
        [TestCase("https://www.x.com")]
        [TestCase("https://ab3.army")]
        [TestCase("https://www.instagram.com")]
        [TestCase("https://www.google.com/maps/place/Cock+Island/@53.8376334,-9.3719559,14z")]
        public void IsValidLinkTest(string link)
        {
            Assert.That(UpdateHandlers.IsValidLink(link), Is.True);
        }

        [TestCase("www.google.com")]
        [TestCase("random string")]
        [TestCase("123@gmail.com")]
        public void IsValidLinkTestFailsForInvalidLink(string link)
        {
            Assert.That(UpdateHandlers.IsValidLink(link), Is.False);
        }
    }
}