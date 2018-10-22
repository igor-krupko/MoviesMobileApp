using System.Collections.Generic;
using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Genres
{
    public class GenresListDto
    {
        [JsonProperty("genres")]
        public IEnumerable<GenreDto> Genres { get; set; }
    }
}
