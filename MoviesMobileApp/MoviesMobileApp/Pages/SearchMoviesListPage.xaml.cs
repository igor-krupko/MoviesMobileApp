using Xamarin.Forms;

namespace MoviesMobileApp.Pages
{
    public partial class SearchMoviesListPage : ContentPage
    {
	    public SearchMoviesListPage()
	    {
		    InitializeComponent();
	    }

        protected override void OnAppearing()
        {
            SearchBarView.Focus();
        }
    }
}