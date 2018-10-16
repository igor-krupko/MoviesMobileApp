using System.Net.Http;
using Refit;

namespace MoviesMobileApp.Services
{
    public class RestServiceWrapper
    {
        private readonly HttpClient httpClient;

        public RestServiceWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public TApi For<TApi>()
        {
            return RestService.For<TApi>(httpClient);
        }
    }
}