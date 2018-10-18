using System;
using System.Linq;
using System.Reactive.Linq;
using MoviesMobileApp.Api.Configuration;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.Pages;
using MoviesMobileApp.Services.Configuration;
using MoviesMobileApp.Services.Movies;
using MoviesMobileApp.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MoviesListPageViewModel : BindableBase, INavigatedAware
    {
        private readonly INavigationService navigationService;
        private readonly IMoviesService moviesService;
        private readonly IConfigurationService configurationService;

        private IDisposable itemsLoadingSubscription;

        private int currentPageNumber;
        private bool canLoadMore = true;

        public LoadingInfo LoadingInfo { get; set; } = new LoadingInfo();
        public ExtendedObservableCollection<MovieListItemViewModel> Items { get; set; }
        public DelegateCommand ReloadItemsCommand { get; }
        public DelegateCommand<MovieListItemViewModel> OnItemTappedCommand { get; }
        public DelegateCommand LoadMoreItemsCommand { get; }

        public MoviesListPageViewModel(INavigationService navigationService,
            IMoviesService moviesService,
            IConfigurationService configurationService)
        {
            this.navigationService = navigationService;
            this.moviesService = moviesService;
            this.configurationService = configurationService;

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

            itemsLoadingSubscription = configurationService.GetConfiguration()
                .CombineLatest(moviesService.GetUpcomingMovies(page), (dto, moviesDto) => new Tuple<ConfigurationDto, UpcomingMoviesDto>(dto, moviesDto))
                .Subscribe(
                    moviesInfo => OnMoviesInfoLoaded(moviesInfo, reset),
                    OnErrorLoadingItems);
        }

        private void OnMoviesInfoLoaded(Tuple<ConfigurationDto, UpcomingMoviesDto> result, bool reset)
        {
            var movies = result.Item2.UpcomingMovies?.ToList();

            if (movies != null)
            {
                canLoadMore = result.Item2.TotalPagesCount != currentPageNumber;

                if (reset || Items == null)
                {
                    Items = ObservableCollectionHelper.CreateFrom<MovieListItemViewModel>(movies, movie => SetBaseImagePath(result.Item1.ImagesConfiguration, movie));
                }
                else
                {
                    Items.UpdateFrom(movies, movie => SetBaseImagePath(result.Item1.ImagesConfiguration, movie));
                }

                FinishLoading();
            }

            LoadingInfo.HasDataToShow = Items != null && Items.Any();
        }

        private void SetBaseImagePath(ImagesConfigurationDto imagesConfiguration, MovieListItemViewModel movie)
        {
            movie.BaseImagePath = $"{imagesConfiguration.ImageBaseUrl}/{imagesConfiguration.PosterSizes.First()}";
        }

        private void FinishLoading()
        {
            LoadingInfo.FinishLoading();
        }
    }
}
