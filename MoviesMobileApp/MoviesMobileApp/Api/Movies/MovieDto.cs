using System;
using System.Collections.Generic;
using MoviesMobileApp.Common;
using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Movies
{
    public class MovieDto : IIdentifiable
    {
        public object Identity => Id;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate{ get; set; }

        [JsonProperty("genre_ids")]
        public IEnumerable<int> GenreIds { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }
}