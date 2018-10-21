using Prism.Mvvm;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public class BasePageViewModel : BindableBase, INavigationAware
    {
        private const string NavigationModeKey = "__NavigationMode";

        public bool IsNavigatedBack { get; private set; }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            SetNavigatingBackIndicator(parameters);
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            SetNavigatingBackIndicator(parameters);
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            SetNavigatingBackIndicator(parameters);
        }

        private void SetNavigatingBackIndicator(NavigationParameters parameters)
        {
            IsNavigatedBack = parameters.ContainsKey(NavigationModeKey)
                ? (NavigationMode)parameters[NavigationModeKey] == NavigationMode.Back
                : IsNavigatedBack;
        }
    }
}
