using System;
using Akavache;
using MoviesMobileApp.Api.Configuration;

namespace MoviesMobileApp.Services.Configuration
{
    public class ConfigurationService : BaseService, IConfigurationService
    {
        private readonly TimeSpan expirationTimeSpan = TimeSpan.FromDays(1);
        private readonly IRestServiceResolver restServiceResolver;

        public ConfigurationService(IRestServiceResolver restServiceResolver)
        {
            this.restServiceResolver = restServiceResolver;
        }

        public IObservable<ConfigurationDto> GetConfiguration()
        {
            var api = restServiceResolver.For<IConfigurationApi>();

            return Cache.GetAndFetchLatest($"{nameof(GetConfiguration)}",
                WithDefaultPolicy(() => api.GetConfiguration(ApplicationConfiguration.ApiKey)),
                offset => DateTimeOffset.Now - offset > expirationTimeSpan);
        }
    }
}