using System.Collections.Generic;
using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Configuration
{
    public class ImagesConfigurationDto
    {
        [JsonProperty("secure_base_url")]
        public string ImageBaseUrl { get; set; }

        [JsonProperty("poster_sizes")]
        public IEnumerable<string> PosterSizes { get; set; }
    }
}