using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Genres
{
    public class GenreDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}