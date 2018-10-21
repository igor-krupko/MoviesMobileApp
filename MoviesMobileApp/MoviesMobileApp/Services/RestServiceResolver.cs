using System;
using System.Net.Http;
using ModernHttpClient;
using MoviesMobileApp.Api;
using Plugin.Connectivity.Abstractions;

namespace MoviesMobileApp.Services
{
    public class RestServiceResolver : IRestServiceResolver
    {
        private static readonly TimeSpan DefaultHttpTimeout = TimeSpan.FromSeconds(30);
        
        private readonly IConnectivity connectivity;
        private readonly RestServiceWrapper restServiceWrapper;

        public RestServiceResolver(IConnectivity connectivity)
        {
            this.connectivity = connectivity;

            restServiceWrapper = GetServiceWrapper();
        }

        public TApi For<TApi>()
        {
            return restServiceWrapper.For<TApi>();
        }

        private RestServiceWrapper GetServiceWrapper()
        {
            var client = CreateHttpClient(new NativeMessageHandler());
            return new RestServiceWrapper(client);
        }

        private HttpClient CreateHttpClient(HttpMessageHandler innerHandler)
        {
            HttpMessageHandler handler = new ConnectivityCheckHandler(connectivity, innerHandler);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(ApplicationConfiguration.ApiBaseUrl),
                Timeout = DefaultHttpTimeout
            };

            return client;
        }
    }
}
