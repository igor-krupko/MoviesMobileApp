using System;
using MoviesMobileApp.Api;

namespace MoviesMobileApp.Services
{
    public interface IMoviesService
    {
        IObservable<UpcomingMoviesDto> GetUpcomingMovies(int pageNumber);
    }
}