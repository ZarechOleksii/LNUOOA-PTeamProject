using Microsoft.Extensions.Options;
using Moq;
using SharedLib.Extensions;

namespace SharedLib.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetConfiguration_ReturnsCorrect()
        {
            var defaultConfig = new MyConfiguration() { Bar = 1, Foo = "foo" };

            var serviceProviderMock = new Mock<IServiceProvider>();
            var optionsMock = new Mock<IOptions<MyConfiguration>>();
            optionsMock.Setup(o => o.Value).Returns(defaultConfig);
            serviceProviderMock.Setup(s => s.GetService(typeof(IOptions<MyConfiguration>))).Returns(optionsMock.Object);

            var result = serviceProviderMock.Object.GetConfiguration<MyConfiguration>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result == defaultConfig, Is.True);
        }

        [Test]
        public void GetConfiguration_ReturnsNull()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(s => s.GetService(typeof(IOptions<MyConfiguration>))).Returns(null);

            Assert.Throws<ArgumentNullException>(() => serviceProviderMock.Object.GetConfiguration<MyConfiguration>());
        }
    }

    public class MyConfiguration
    {
        public string Foo { get; set; }
        public int Bar { get; set; }
    }
}