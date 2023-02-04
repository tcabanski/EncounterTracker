using FluentAssertions;

namespace EncounterTracker.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            int x = 1;
            x.Should().Be(1);
        }
    }
}