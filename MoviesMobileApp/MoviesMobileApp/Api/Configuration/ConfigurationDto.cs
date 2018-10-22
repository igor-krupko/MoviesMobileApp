using Newtonsoft.Json;

namespace MoviesMobileApp.Api.Configuration
{
    public class ConfigurationDto
    {
        [JsonProperty("images")]
        public ImagesConfigurationDto ImagesConfiguration { get; set; }
    }
}