using AutoMapper;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.ViewModels;

namespace MoviesMobileApp.Mappers
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<MovieDto, MovieViewModel>()
                .ForMember(dest => dest.Genres, opts => opts.Ignore())
                .ForMember(dest => dest.BaseImagePath, opts => opts.Ignore())
                .ForMember(dest => dest.PosterSizes, opts => opts.Ignore());
        }
    }
}