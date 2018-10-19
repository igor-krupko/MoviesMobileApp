using System;
using System.Collections.Generic;
using MoviesMobileApp.Common;

namespace MoviesMobileApp.ViewModels
{
    public class MovieListItemViewModel : BaseViewModel, IIdentifiable
    {
        public object Identity => Id;

        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public DateTime ReleaseDate { get; set; }

        public string Genres { get; set; }
        
        public IEnumerable<int> GenreIds { get; set; }
        
        public string Overview { get; set; }
        
        public string PosterPath { get; set; }

        public string BaseImagePath { get; set; }

        public string FullPosterPath => $"{BaseImagePath}/{PosterPath}";
    }
}
