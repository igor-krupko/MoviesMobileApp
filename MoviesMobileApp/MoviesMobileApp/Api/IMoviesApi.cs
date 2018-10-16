using System.Threading.Tasks;
using Refit;

namespace MoviesMobileApp.Api
{
    public interface IMoviesApi
    {
        [Get("/movie/upcoming?api_key={apiKey}&page={pageNumber}")]
        Task<UpcomingMoviesDto> GetUpcomingMovies(string apiKey, int pageNumber);
    }
}
