using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MoviesMobileApp.Utils
{
    public static class ImageSizeTransformator
    {
        private const string OriginalImageSizeName = "original";

        public static string GetImageSizeWithRequireQuality(IEnumerable<string> allSizes, int requiredQuality)
        {
            return allSizes.Select(imageSize => new KeyValuePair<string, int>(imageSize, GetImageQuality(imageSize)))
                .OrderBy(pair => pair.Value)
                .First(pair => pair.Value >= requiredQuality)
                .Key;
        }

        private static int GetImageQuality(string imageSize)
        {
            return imageSize == OriginalImageSizeName
                ? int.MaxValue
                : int.Parse(Regex.Match(imageSize, @"\d+").Value);
        }
    }
}