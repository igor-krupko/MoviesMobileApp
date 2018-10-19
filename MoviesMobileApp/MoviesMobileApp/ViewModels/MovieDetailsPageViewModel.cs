using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MovieDetailsPageViewModel : BasePageViewModel
    {
        public const string SelectedMovieKey = "SelectedMovie";

        public MovieListItemViewModel Movie { get; set; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(SelectedMovieKey))
            {
                Movie = (MovieListItemViewModel)parameters[SelectedMovieKey];
            }
        }
    }
}
