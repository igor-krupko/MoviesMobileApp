using System;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.Pages;
using MoviesMobileApp.Services.Configuration;
using MoviesMobileApp.Services.Genres;
using MoviesMobileApp.Services.Movies;
using Prism.Commands;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MoviesListPageViewModel : BaseMoviesListPageViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IMoviesService moviesService;

        public DelegateCommand OpenSearchCommand { get; }

        public MoviesListPageViewModel(INavigationService navigationService,
            IGenresService genresService, 
            IConfigurationService configurationService,
            IMoviesService moviesService)
            : base(navigationService, genresService, configurationService)
        {
            this.navigationService = navigationService;
            this.moviesService = moviesService;

            OpenSearchCommand = new DelegateCommand(OpenSearch);
        }

        public async void OpenSearch()
        {
            await navigationService.NavigateAsync(nameof(SearchMoviesListPage));
        }

        protected override IObservable<MoviesDto> GetMoviesObservable()
        {
            return moviesService.GetUpcomingMovies(CurrentPageNumber);
        }
    }
}