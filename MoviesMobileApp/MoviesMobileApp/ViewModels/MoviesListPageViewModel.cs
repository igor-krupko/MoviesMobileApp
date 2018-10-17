using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoviesMobileApp.Api;
using MoviesMobileApp.Pages;
using MoviesMobileApp.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MoviesListPageViewModel : BindableBase, INavigatedAware
    {
        private readonly INavigationService navigationService;
        private readonly IMoviesService moviesService;

        private IDisposable itemsLoadingSubscription;

        private int currentPageNumber;
        private bool canLoadMore = true;

        public LoadingInfo LoadingInfo { get; set; } = new LoadingInfo();
        public ObservableCollection<MovieListItemViewModel> Items { get; set; }
        public DelegateCommand ReloadItemsCommand { get; }
        public DelegateCommand<MovieListItemViewModel> OnItemTappedCommand { get; }
        public DelegateCommand LoadMoreItemsCommand { get; }

        public MoviesListPageViewModel(INavigationService navigationService,
            IMoviesService moviesService)
        {
            this.navigationService = navigationService;
            this.moviesService = moviesService;

            ReloadItemsCommand = new DelegateCommand(ReloadItems);
            OnItemTappedCommand = new DelegateCommand<MovieListItemViewModel>(OnItemTapped);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems);
        }

        public async void OnItemTapped(MovieListItemViewModel item)
        {
            await navigationService.NavigateAsync(nameof(MovieDetailsPage));
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            itemsLoadingSubscription?.Dispose();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            ReloadItems();
        }

        public void ReloadItems()
        {
            LoadingInfo.StartRefreshing();

            currentPageNumber = 1;
            LoadItems(1, true);
        }

        public void LoadMoreItems()
        {
            if (!canLoadMore)
            {
                return;
            }

            LoadingInfo.StartLoadingMore();

            LoadItems(++currentPageNumber, false);
        }

        protected virtual void OnErrorLoadingItems(Exception e)
        {
            LoadingInfo.HasDataToShow = Items != null && Items.Any();
            FinishLoading();
        }

        private void LoadItems(int page, bool reset)
        {
            itemsLoadingSubscription?.Dispose();

            itemsLoadingSubscription = moviesService.GetUpcomingMovies(page)
                .Subscribe(
                    items => OnItemsLoaded(items, reset),
                    OnErrorLoadingItems);
        }

        private void OnItemsLoaded(UpcomingMoviesDto upcomingMoviesDto, bool reset)
        {
            var list = upcomingMoviesDto.UpcomingMovies?.ToList();

            if (list != null)
            {
                canLoadMore = upcomingMoviesDto.TotalPagesCount != currentPageNumber;

                Items = new ObservableCollection<MovieListItemViewModel>(list.Select(x => new MovieListItemViewModel
                {
                    Title = x.Title,
                    Overview = x.Overview,
                    ReleaseDate = x.ReleaseDate
                }));

                FinishLoading();
            }

            LoadingInfo.HasDataToShow = Items != null && Items.Any();
        }

        private void FinishLoading()
        {
            LoadingInfo.FinishLoading();
        }
    }
}
