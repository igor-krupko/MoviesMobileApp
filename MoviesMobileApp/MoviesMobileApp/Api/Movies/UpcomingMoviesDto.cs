using System.Collections.Generic;
using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Movies
{
    public class UpcomingMoviesDto
    {
        [JsonProperty("results")]
        public IEnumerable<MovieDto> UpcomingMovies { get; set; }

        [JsonProperty("total_results")]
        public int TotalResultsCount { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPagesCount { get; set; }
    }
}