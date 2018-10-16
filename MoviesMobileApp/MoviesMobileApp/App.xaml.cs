using MoviesMobileApp.Pages;
using MoviesMobileApp.Services;
using MoviesMobileApp.ViewModels;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
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
            
            await NavigationService.NavigateAsync($"{nameof(RootPage)}/{nameof(MoviesListPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<IRestServiceResolver, RestServiceResolver>();
            container.RegisterSingleton<IMoviesService, MoviesService>();

            container.RegisterForNavigation<RootPage>();
            container.RegisterForNavigation<MoviesListPage, MoviesListPageViewModel>();
            container.RegisterForNavigation<MovieDetailsPage, MovieDetailsPageViewModel>();
        }
    }
}