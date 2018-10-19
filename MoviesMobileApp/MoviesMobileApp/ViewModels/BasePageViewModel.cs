using Prism.Mvvm;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class BasePageViewModel : BindableBase, INavigatedAware
    {
        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }
    }
}
