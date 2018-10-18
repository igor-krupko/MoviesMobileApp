using AutoMapper;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.ViewModels;

namespace MoviesMobileApp.Mappers
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<MovieDto, MovieListItemViewModel>()
                .ForMember(dest => dest.BaseImagePath, opts => opts.Ignore());
        }
    }
}