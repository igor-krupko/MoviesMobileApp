using System.Collections.Generic;
using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Movies
{
    public class MoviesDto
    {
        [JsonProperty("results")]
        public IEnumerable<MovieDto> Movies { get; set; }

        [JsonProperty("total_results")]
        public int TotalResultsCount { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPagesCount { get; set; }

        public static MoviesDto Empty => new MoviesDto
        {
            Movies = new List<MovieDto>(),
            TotalPagesCount = 1,
            TotalResultsCount = 0
        };
    }
}