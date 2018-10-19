using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MoviesMobileApp.Api.Configuration;
using MoviesMobileApp.Api.Genres;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.Pages;
using MoviesMobileApp.Services.Configuration;
using MoviesMobileApp.Services.Genres;
using MoviesMobileApp.Services.Movies;
using MoviesMobileApp.Utils;
using Prism.Commands;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MoviesListPageViewModel : BasePageViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IMoviesService moviesService;
        private readonly IGenresService genresService;
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
            IGenresService genresService,
            IConfigurationService configurationService)
        {
            this.navigationService = navigationService;
            this.moviesService = moviesService;
            this.genresService = genresService;
            this.configurationService = configurationService;

            ReloadItemsCommand = new DelegateCommand(ReloadItems);
            OnItemTappedCommand = new DelegateCommand<MovieListItemViewModel>(OnItemTapped);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems);
        }

        public async void OnItemTapped(MovieListItemViewModel item)
        {
            await navigationService.NavigateAsync(nameof(MovieDetailsPage),
                new NavigationParameters
                {
                    { MovieDetailsPageViewModel.SelectedMovieKey, item }
                });
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            itemsLoadingSubscription?.Dispose();
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ReloadItems();
        }

        public void ReloadItems()
        {
            LoadingInfo.StartRefreshing();

            currentPageNumber = 1;
            LoadItems(currentPageNumber, true);
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

            var getConfigurationObservable = configurationService.GetConfiguration();
            var getGenresObservable = genresService.GetGenres();
            var getMoviesObservable = moviesService.GetUpcomingMovies(page);

            itemsLoadingSubscription = getConfigurationObservable.CombineLatest(getGenresObservable, getMoviesObservable,
                    (configurationDto, genresDto, moviesDto) => new MoviesListPageData(configurationDto, genresDto, moviesDto))
                .Subscribe(
                    loadedResult => OnMoviesInfoLoaded(loadedResult, reset),
                    OnErrorLoadingItems);
        }

        private void OnMoviesInfoLoaded(MoviesListPageData loadedResult, bool reset)
        {
            var movies = loadedResult.Movies?.ToList();

            if (movies != null)
            {
                canLoadMore = loadedResult.TotalPagesCount != currentPageNumber;

                if (reset || Items == null)
                {
                    Items = ObservableCollectionHelper.CreateFrom<MovieListItemViewModel>(movies, movie => PrepareMovieItemViewModel(loadedResult, movie));
                }
                else
                {
                    Items.UpdateFrom(movies, movie => PrepareMovieItemViewModel(loadedResult, movie));
                }

                FinishLoading();
            }

            LoadingInfo.HasDataToShow = Items != null && Items.Any();
        }

        private void PrepareMovieItemViewModel(MoviesListPageData loadedResult, MovieListItemViewModel movie)
        {
            SetGenres(loadedResult.Genres, movie);
            SetBaseImagePath(loadedResult.ImagesConfiguration, movie);
        }

        private void SetGenres(IEnumerable<GenreDto> genres, MovieListItemViewModel movie)
        {
            movie.Genres = string.Join(", ", movie.GenreIds.Select(id => genres.FirstOrDefault(genre => genre.Id == id)?.Name));
        }

        private void SetBaseImagePath(ImagesConfigurationDto imagesConfiguration, MovieListItemViewModel movie)
        {
            movie.BaseImagePath = $"{imagesConfiguration.ImageBaseUrl}/{imagesConfiguration.PosterSizes.First()}";
        }

        private void FinishLoading()
        {
            LoadingInfo.FinishLoading();
        }

        private class MoviesListPageData
        {
            public ImagesConfigurationDto ImagesConfiguration { get; }
            public IEnumerable<GenreDto> Genres { get; }
            public IEnumerable<MovieDto> Movies { get; }
            public int TotalPagesCount { get; }

            public MoviesListPageData(
                ConfigurationDto configurationDto,
                GenresListDto genresListDto,
                UpcomingMoviesDto upcomingMoviesDto)
            {
                ImagesConfiguration = configurationDto.ImagesConfiguration;
                Genres = genresListDto.Genres;
                Movies = upcomingMoviesDto.UpcomingMovies;
                TotalPagesCount = upcomingMoviesDto.TotalPagesCount;
            }
        }
    }
}