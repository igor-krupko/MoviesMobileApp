using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using MoviesMobileApp.Api;
using MoviesMobileApp.Pages;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MoviesListPageViewModel : BaseListPageViewModel<MovieListItemViewModel, MovieDto>
    {
        private readonly INavigationService navigationService;

        public MoviesListPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override async void OnItemTapped(MovieListItemViewModel item)
        {
            await navigationService.NavigateAsync(nameof(MovieDetailsPage));
        }

        protected override IObservable<IEnumerable<MovieDto>> GetItemsObservable(int pageNumber, int pageSize)
        {
            return Task.Run(() => new List<MovieDto>{new MovieDto {Name = "Movie1"}}).ToObservable();
        }
    }
}
