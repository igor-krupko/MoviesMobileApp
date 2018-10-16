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

        private RestServiceWrapper userInitiatedWrapper;
        private RestServiceWrapper backgroundWrapper;
        private RestServiceWrapper speculativeWrapper;

        private HttpClient userInitiatedClient;
        private HttpClient backgroundClient;
        private HttpClient speculativeClient;

        public RestServiceWrapper UserInitiated => GetServiceWrapper(ref userInitiatedWrapper, ref userInitiatedClient, new NativeMessageHandler());
        public RestServiceWrapper Background => GetServiceWrapper(ref backgroundWrapper, ref backgroundClient, new NativeMessageHandler());
        public RestServiceWrapper Speculative => GetServiceWrapper(ref speculativeWrapper, ref speculativeClient, new NativeMessageHandler());

        public RestServiceResolver()
        {
            this.connectivity = connectivity;
        }

        public TApi For<TApi>(RequestPriority priority)
        {
            switch (priority)
            {
                case RequestPriority.UserInitiated:
                    return UserInitiated.For<TApi>();
                case RequestPriority.Background:
                    return Background.For<TApi>();
                case RequestPriority.Speculative:
                    return Speculative.For<TApi>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
            }
        }

        private RestServiceWrapper GetServiceWrapper(ref RestServiceWrapper wrapper, ref HttpClient httpClient, HttpMessageHandler handler)
        {
            if (wrapper == null)
            {
                if (httpClient == null)
                {
                    httpClient = CreateHttpClient(handler);
                }
                wrapper = new RestServiceWrapper(httpClient);
            }
            return wrapper;
        }

        public HttpClient CreateHttpClient(HttpMessageHandler innerHandler)
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
