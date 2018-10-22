using System;
using MoviesMobileApp.Api.Movies;

namespace MoviesMobileApp.Services.Movies
{
    public interface IMoviesService
    {
        IObservable<MoviesDto> GetUpcomingMovies(int pageNumber);

        IObservable<MoviesDto> SearchMovies(int pageNumber, string query);
    }
}