using System;
using MoviesMobileApp.Api.Configuration;

namespace MoviesMobileApp.Services.Configuration
{
    public interface IConfigurationService
    {
        IObservable<ConfigurationDto> GetConfiguration();
    }
}