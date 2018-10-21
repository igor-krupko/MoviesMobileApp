using System.Linq;
using MoviesMobileApp.Utils;
using NUnit.Framework;

namespace MoviesMobileApp.Tests
{
    [TestFixture]
    public class ImageSizeTransformatorTests
    {
        [TestCase(5, "w10", "w20", "w30", "original", ExpectedResult = "w10")]
        [TestCase(15, "w10", "w20", "w30", "original", ExpectedResult = "w20")]
        [TestCase(20, "w10", "w20", "w30", "original", ExpectedResult = "w20")]
        [TestCase(35, "w10", "w20", "w30", "original", ExpectedResult = "original")]
        public string GetImageSizeWithRequireQualityTest(int requiredQuality, params string[] allSizes)
        {
            return ImageSizeTransformator.GetImageSizeWithRequireQuality(allSizes.ToList(), requiredQuality);
        }
    }
}