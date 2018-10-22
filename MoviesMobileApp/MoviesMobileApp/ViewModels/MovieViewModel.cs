using System;
using System.Collections.Generic;
using MoviesMobileApp.Common;
using MoviesMobileApp.Utils;

namespace MoviesMobileApp.ViewModels
{
    public class MovieViewModel : BaseViewModel, IIdentifiable
    {
        private const int MediumQuality = 160;

        public object Identity => Id;

        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public DateTime? ReleaseDate { get; set; }

        public string Genres { get; set; }
        
        public IEnumerable<int> GenreIds { get; set; }
        
        public string Overview { get; set; }
        
        public string PosterPath { get; set; }

        public string BaseImagePath { get; set; }

        public IEnumerable<string> PosterSizes { get; set; }

        public string MediumQualityPosterPath => $"{BaseImagePath}/{GetImageSize(MediumQuality)}/{PosterPath}";

        public bool HasGenres => !string.IsNullOrWhiteSpace(Genres);

        public bool HasReleaseDate => ReleaseDate.HasValue;

        private string GetImageSize(int quality)
        {
            return ImageSizeTransformator.GetImageSizeWithRequireQuality(PosterSizes, quality);
        }
    }
}
