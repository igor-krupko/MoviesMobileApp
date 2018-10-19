using MoviesMobileApp.Mappers;
using MoviesMobileApp.Pages;
using MoviesMobileApp.Services;
using MoviesMobileApp.Services.Configuration;
using MoviesMobileApp.Services.Genres;
using MoviesMobileApp.Services.Movies;
using MoviesMobileApp.ViewModels;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;

namespace MoviesMobileApp
{
	public partial class App : PrismApplication
	{
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            MappingConfigurator.Configure();

            await NavigationService.NavigateAsync($"{nameof(RootPage)}/{nameof(MoviesListPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<IRestServiceResolver, RestServiceResolver>();
            container.RegisterSingleton<IMoviesService, MoviesService>();
            container.RegisterSingleton<IConfigurationService, ConfigurationService>();
            container.RegisterSingleton<IGenresService, GenresService>();

            container.RegisterForNavigation<RootPage>();
            container.RegisterForNavigation<MoviesListPage, MoviesListPageViewModel>();
            container.RegisterForNavigation<MovieDetailsPage, MovieDetailsPageViewModel>();
        }
    }
}