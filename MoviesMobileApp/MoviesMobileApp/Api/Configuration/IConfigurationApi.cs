using System.Threading.Tasks;
using Refit;

namespace MoviesMobileApp.Api.Configuration
{
    public interface IConfigurationApi
    {
        [Get("/configuration?api_key={apiKey}")]
        Task<ConfigurationDto> GetConfiguration(string apiKey);
    }
}
