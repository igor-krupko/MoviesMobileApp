using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MoviesMobileApp.Api.Configuration;
using MoviesMobileApp.Api.Genres;
using MoviesMobileApp.Api.Movies;
using MoviesMobileApp.Pages;
using MoviesMobileApp.Services.Configuration;
using MoviesMobileApp.Services.Genres;
using MoviesMobileApp.Utils;
using Prism.Commands;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public abstract class BaseMoviesListPageViewModel : BasePageViewModel
    {
        protected int CurrentPageNumber;

        private readonly INavigationService navigationService;
        private readonly IGenresService genresService;
        private readonly IConfigurationService configurationService;

        private IDisposable itemsLoadingSubscription;
        
        private bool canLoadMore = true;

        public LoadingInfo LoadingInfo { get; set; } = new LoadingInfo();
        public ExtendedObservableCollection<MovieViewModel> Items { get; set; }
        public DelegateCommand ReloadItemsCommand { get; }
        public DelegateCommand<MovieViewModel> OnItemTappedCommand { get; }
        public DelegateCommand LoadMoreItemsCommand { get; }

        protected BaseMoviesListPageViewModel(INavigationService navigationService,
            IGenresService genresService,
            IConfigurationService configurationService)
        {
            this.navigationService = navigationService;
            this.genresService = genresService;
            this.configurationService = configurationService;

            ReloadItemsCommand = new DelegateCommand(ReloadItems);
            OnItemTappedCommand = new DelegateCommand<MovieViewModel>(OnItemTapped);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems);
        }

        public async void OnItemTapped(MovieViewModel item)
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

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            if (!IsNavigatedBack)
            {
                ReloadItems();
            }
        }

        public void ReloadItems()
        {
            LoadingInfo.StartRefreshing();

            CurrentPageNumber = 1;
            LoadItems(true);
        }

        public void LoadMoreItems()
        {
            if (!canLoadMore)
            {
                return;
            }

            LoadingInfo.StartLoadingMore();

            CurrentPageNumber++;
            LoadItems(false);
        }

        protected abstract IObservable<MoviesDto> GetMoviesObservable();

        private void LoadItems(bool reset)
        {
            itemsLoadingSubscription?.Dispose();

            var getConfigurationObservable = configurationService.GetConfiguration();
            var getGenresObservable = genresService.GetGenres();
            var getMoviesObservable = GetMoviesObservable();

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
                using (Disposable.Create(FinishLoading))
                {
                    canLoadMore = loadedResult.TotalPagesCount != CurrentPageNumber;

                    if (reset || Items == null)
                    {
                        Items = ObservableCollectionHelper.CreateFrom<MovieViewModel>(movies,
                            movie => PrepareMovieItemViewModel(loadedResult, movie));
                    }
                    else
                    {
                        Items.UpdateFrom(movies, movie => PrepareMovieItemViewModel(loadedResult, movie));
                    }
                }
            }

            LoadingInfo.HasDataToShow = Items != null && Items.Any();
        }

        private void OnErrorLoadingItems(Exception e)
        {
            LoadingInfo.HasDataToShow = Items != null && Items.Any();
            FinishLoading();
        }

        private void PrepareMovieItemViewModel(MoviesListPageData loadedResult, MovieViewModel movie)
        {
            SetGenres(loadedResult.Genres, movie);
            SetImagesConfigs(loadedResult.ImagesConfiguration, movie);
        }

        private void SetGenres(IEnumerable<GenreDto> genres, MovieViewModel movie)
        {
            movie.Genres = string.Join(", ", movie.GenreIds.Select(id => genres.FirstOrDefault(genre => genre.Id == id)?.Name));
        }

        private void SetImagesConfigs(ImagesConfigurationDto imagesConfiguration, MovieViewModel movie)
        {
            movie.BaseImagePath = imagesConfiguration.ImageBaseUrl;
            movie.PosterSizes = imagesConfiguration.PosterSizes;
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
                MoviesDto moviesDto)
            {
                ImagesConfiguration = configurationDto.ImagesConfiguration;
                Genres = genresListDto.Genres;
                Movies = moviesDto.Movies;
                TotalPagesCount = moviesDto.TotalPagesCount;
            }
        }
    }
}