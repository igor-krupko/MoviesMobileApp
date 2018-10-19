using System;
using MoviesMobileApp.Api.Genres;

namespace MoviesMobileApp.Services.Genres
{
    public interface IGenresService
    {
        IObservable<GenresListDto> GetGenres();
    }
}