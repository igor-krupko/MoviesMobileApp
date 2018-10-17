using System;
using MoviesMobileApp.Api.Movies;

namespace MoviesMobileApp.Services.Movies
{
    public interface IMoviesService
    {
        IObservable<UpcomingMoviesDto> GetUpcomingMovies(int pageNumber);
    }
}