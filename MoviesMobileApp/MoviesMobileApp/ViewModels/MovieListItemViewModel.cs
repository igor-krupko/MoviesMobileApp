﻿using System;
using System.Collections.Generic;

namespace MoviesMobileApp.ViewModels
{
    public class MovieListItemViewModel : BaseViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public DateTime ReleaseDate { get; set; }
        
        public IEnumerable<int> GenreIds { get; set; }
        
        public string Overview { get; set; }
        
        public string PosterPath { get; set; }
    }
}
