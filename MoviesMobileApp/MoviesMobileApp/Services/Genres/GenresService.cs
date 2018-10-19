using System;
using Akavache;
using MoviesMobileApp.Api.Genres;

namespace MoviesMobileApp.Services.Genres
{
    public class GenresService : BaseService, IGenresService
    {
        private readonly TimeSpan expirationTimeSpan = TimeSpan.FromDays(1);
        private readonly IRestServiceResolver restServiceResolver;

        public GenresService(IRestServiceResolver restServiceResolver)
        {
            this.restServiceResolver = restServiceResolver;
        }

        public IObservable<GenresListDto> GetGenres()
        {
            var api = restServiceResolver.For<IGenresApi>();

            return Cache.GetAndFetchLatest($"{nameof(GetGenres)}",
                WithDefaultPolicy(() => api.GetGenres(ApplicationConfiguration.ApiKey)),
                offset => DateTimeOffset.Now - offset > expirationTimeSpan);
        }
    }
}