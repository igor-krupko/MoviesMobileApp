using System;
using Akavache;
using MoviesMobileApp.Api.Movies;

namespace MoviesMobileApp.Services.Movies
{
    public class MoviesService : BaseService, IMoviesService
    {
        private readonly IRestServiceResolver restServiceResolver;

        public MoviesService(IRestServiceResolver restServiceResolver)
        {
            this.restServiceResolver = restServiceResolver;
        }

        public IObservable<MoviesDto> GetUpcomingMovies(int pageNumber)
        {
            var api = restServiceResolver.For<IMoviesApi>();

            return Cache.GetAndFetchLatest($"{nameof(GetUpcomingMovies)}/{pageNumber}",
                WithDefaultPolicy(() => api.GetUpcomingMovies(ApplicationConfiguration.ApiKey, pageNumber)),
                offset => true);
        }

        public IObservable<MoviesDto> SearchMovies(int pageNumber, string query)
        {
            var api = restServiceResolver.For<IMoviesApi>();

            return Cache.GetAndFetchLatest($"{nameof(SearchMovies)}/{query}/{pageNumber}",
                WithDefaultPolicy(() => api.SearchMovies(ApplicationConfiguration.ApiKey, query, pageNumber)),
                offset => true,
                DateTimeOffset.Now.AddHours(1));
        }
    }
}