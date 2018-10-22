using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MoviesMobileApp.Services;
using Plugin.Connectivity.Abstractions;

namespace MoviesMobileApp.Api
{
    public class ConnectivityCheckHandler : DelegatingHandler
    {
        private readonly IConnectivity connectivity;

        public ConnectivityCheckHandler(IConnectivity connectivity, HttpMessageHandler inner)
            : base(inner)
        {
            this.connectivity = connectivity;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!connectivity.IsConnected)
            {
                throw new NetworkProblemException("Check failed: there no active connections to send request over.");
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}