using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace MoviesMobileApp.Views
{
    public class ExtendedListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(ExtendedListView));
        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(ExtendedListView));

        public ICommand LoadMoreCommand
        {
            get => (ICommand)GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        public ICommand ItemTappedCommand
        {
            get => (ICommand)GetValue(ItemTappedCommandProperty);
            set => SetValue(ItemTappedCommandProperty, value);
        }

        public ExtendedListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
            ItemTapped += OnItemTapped;
            ItemAppearing += OnItemAppearing;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemTappedCommand != null && ItemTappedCommand.CanExecute(e.Item))
            {
                ItemTappedCommand.Execute(e.Item);
                SelectedItem = null;
            }
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemsSource is IList items && e.Item == items[items.Count - 1] && LoadMoreCommand != null &&
                LoadMoreCommand.CanExecute(null))
            {
                LoadMoreCommand.Execute(null);
            }
        }
    }
}