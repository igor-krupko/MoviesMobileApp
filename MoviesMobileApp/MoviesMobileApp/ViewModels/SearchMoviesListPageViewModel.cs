using System;
using System.Reactive.Linq;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.Services.Configuration;
using MoviesMobileApp.Services.Genres;
using MoviesMobileApp.Services.Movies;
using Prism.Commands;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class SearchMoviesListPageViewModel : BaseMoviesListPageViewModel
    {
        private readonly IMoviesService moviesService;

        private string searchQuery;

        public DelegateCommand<string> SearchCommand { get; }

        public SearchMoviesListPageViewModel(INavigationService navigationService,
            IMoviesService moviesService,
            IGenresService genresService,
            IConfigurationService configurationService)
            : base(navigationService, genresService, configurationService)
        {
            this.moviesService = moviesService;

            SearchCommand = new DelegateCommand<string>(Search);
        }

        protected override IObservable<MoviesDto> GetMoviesObservable()
        {
            return string.IsNullOrWhiteSpace(searchQuery) 
                ? Observable.Return(MoviesDto.Empty)
                : moviesService.SearchMovies(CurrentPageNumber, searchQuery);
        }

        private void Search(string query)
        {
            searchQuery = query;

            ReloadItems();
        }
    }
}