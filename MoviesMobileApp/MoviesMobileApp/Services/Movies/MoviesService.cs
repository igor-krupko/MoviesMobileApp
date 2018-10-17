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

        public IObservable<UpcomingMoviesDto> GetUpcomingMovies(int pageNumber)
        {
            var api = restServiceResolver.For<IMoviesApi>();

            return Cache.GetAndFetchLatest($"{nameof(GetUpcomingMovies)}/{pageNumber}",
                WithDefaultPolicy(() => api.GetUpcomingMovies(ApplicationConfiguration.ApiKey, pageNumber)),
                offset => true);
        }
    }
}