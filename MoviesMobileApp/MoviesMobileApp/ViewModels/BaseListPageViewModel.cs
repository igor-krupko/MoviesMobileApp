using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoviesMobileApp.Api;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace MoviesMobileApp.ViewModels
{
    public abstract class BaseListPageViewModel<TItemViewModel, TDto> : BindableBase, INavigatedAware
        where TItemViewModel : class
    {
        private const int PageSize = 20;

        private IDisposable itemsLoadingSubscription;

        private int pageNumber;
        private bool canLoadMore = true;

        public LoadingInfo LoadingInfo { get; set; } = new LoadingInfo();
        public ObservableCollection<TItemViewModel> Items { get; set; }
        public DelegateCommand ReloadItemsCommand { get; }
        public DelegateCommand<TItemViewModel> OnItemTappedCommand { get; }
        public DelegateCommand LoadMoreItemsCommand { get; }

        protected BaseListPageViewModel()
        {
            ReloadItemsCommand = new DelegateCommand(ReloadItems);
            OnItemTappedCommand = new DelegateCommand<TItemViewModel>(OnItemTapped);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems);
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            itemsLoadingSubscription?.Dispose();
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            ReloadItems();
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void ReloadItems()
        {
            LoadingInfo.StartRefreshing();

            pageNumber = 0;
            LoadItems(0, true);
        }

        public void LoadMoreItems()
        {
            if (!canLoadMore)
            {
                return;
            }

            LoadingInfo.StartLoadingMore();

            LoadItems(++pageNumber, false);
        }

        public virtual void OnItemTapped(TItemViewModel item)
        {
        }

        protected virtual void OnErrorLoadingItems(Exception e)
        {
            LoadingInfo.HasDataToShow = Items != null && Items.Any();
            FinishLoading();
        }

        protected abstract IObservable<IEnumerable<TDto>> GetItemsObservable(int pageNumber, int pageSize);

        private void LoadItems(int page, bool reset)
        {
            itemsLoadingSubscription?.Dispose();

            itemsLoadingSubscription = GetItemsObservable(page, PageSize)
                .Subscribe(
                    items => OnItemsLoaded(items, reset),
                    OnErrorLoadingItems);
        }

        private void OnItemsLoaded(IEnumerable<TDto> items, bool reset)
        {
            var list = items?.ToList();

            if (list != null)
            {
                canLoadMore = list.Any() && list.Count % PageSize == 0;

                Items = new ObservableCollection<TItemViewModel>(list.Select(x => (new MovieListItemViewModel
                {
                    Name = (x as MovieDto)?.Name
                }) as TItemViewModel));

                FinishLoading();
            }

            LoadingInfo.HasDataToShow = Items != null && Items.Any();
        }

        private void FinishLoading()
        {
            LoadingInfo.FinishLoading();
        }
    }
}
