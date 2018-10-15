using System.Windows.Input;
using Xamarin.Forms;

namespace MoviesMobileApp.Views
{
    public class PullToRefreshLayout : ContentView
    {
        public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(PullToRefreshLayout), false);
        public static readonly BindableProperty IsPullToRefreshEnabledProperty = BindableProperty.Create(nameof(IsPullToRefreshEnabled), typeof(bool), typeof(PullToRefreshLayout), true);
        public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(nameof(RefreshCommand), typeof(ICommand), typeof(PullToRefreshLayout));
        public static readonly BindableProperty RefreshColorProperty = BindableProperty.Create(nameof(RefreshColor), typeof(Color), typeof(PullToRefreshLayout), Color.Default);
        public static readonly BindableProperty RefreshBackgroundColorProperty = BindableProperty.Create(nameof(RefreshBackgroundColor), typeof(Color), typeof(PullToRefreshLayout), Color.Default);


        public bool IsRefreshing
        {
            get => (bool)GetValue(IsRefreshingProperty);
            set => SetValue(IsRefreshingProperty, value);
        }

        public bool IsPullToRefreshEnabled
        {
            get => (bool)GetValue(IsPullToRefreshEnabledProperty);
            set => SetValue(IsPullToRefreshEnabledProperty, value);
        }

        public ICommand RefreshCommand
        {
            get => (ICommand)GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }

        public Color RefreshColor
        {
            get => (Color)GetValue(RefreshColorProperty);
            set => SetValue(RefreshColorProperty, value);
        }

        public Color RefreshBackgroundColor
        {
            get => (Color)GetValue(RefreshBackgroundColorProperty);
            set => SetValue(RefreshBackgroundColorProperty, value);
        }

        public PullToRefreshLayout()
        {
            IsClippedToBounds = true;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }
        
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return Content == null
                ? new SizeRequest(new Size(100, 100))
                : Content.Measure(widthConstraint, heightConstraint);
        }
    }
}
