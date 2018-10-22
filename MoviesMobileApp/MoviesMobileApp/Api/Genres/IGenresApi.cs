using System.Threading.Tasks;
using Refit;

namespace MoviesMobileApp.Api.Genres
{
    public interface IGenresApi
    {
        [Get("/genre/movie/list?api_key={apiKey}")]
        Task<GenresListDto> GetGenres(string apiKey);
    }
}
