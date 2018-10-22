using System.Threading.Tasks;
using Refit;

namespace MoviesMobileApp.Api.Movies
{
    public interface IMoviesApi
    {
        [Get("/movie/upcoming?api_key={apiKey}&page={pageNumber}")]
        Task<MoviesDto> GetUpcomingMovies(string apiKey, int pageNumber);

        [Get("/search/movie?api_key={apiKey}&query={query}&page={pageNumber}")]
        Task<MoviesDto> SearchMovies(string apiKey, string query, int pageNumber);
    }
}
