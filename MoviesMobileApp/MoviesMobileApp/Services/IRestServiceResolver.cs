using System.Net.Http;

namespace MoviesMobileApp.Services
{
    public interface IRestServiceResolver
    {
        RestServiceWrapper UserInitiated { get; }
        RestServiceWrapper Background { get; }
        RestServiceWrapper Speculative { get; }

        TApi For<TApi>(RequestPriority priority = RequestPriority.UserInitiated);

        HttpClient CreateHttpClient(HttpMessageHandler innerHandler);
    }
}
