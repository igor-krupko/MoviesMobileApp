using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class MovieDetailsPageViewModel : BasePageViewModel
    {
        public const string SelectedMovieKey = "SelectedMovie";

        public MovieViewModel Movie { get; set; }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            if (parameters.ContainsKey(SelectedMovieKey))
            {
                Movie = (MovieViewModel)parameters[SelectedMovieKey];
            }
        }
    }
}
