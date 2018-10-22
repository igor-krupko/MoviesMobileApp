namespace MoviesMobileApp.ViewModels
{
    public class LoadingInfo : BaseViewModel
    {
        private readonly object locker = new object();

        public bool HasDataToShow { get; set; }

        public bool IsRefreshing { get; set; }
        public bool IsLoadingMore { get; private set; }
        public bool IsFirstLoading { get; private set; } = true;

        public bool IsPullToRefreshEnabled => !IsFirstLoading;
        public bool IsLoadingIndicatorVisible => IsFirstLoading;
        public bool IsDataVisible => HasDataToShow;
        public bool IsNoDataVisible => !IsFirstLoading && !HasDataToShow;

        public void StartRefreshing()
        {
            IsRefreshing = true;
        }

        public void StartLoadingMore()
        {
            IsLoadingMore = true;
        }

        public void FinishLoading()
        {
            lock (locker)
            {
                if (IsLoadingMore)
                {
                    IsLoadingMore = false;
                }
                else
                {
                    if (IsRefreshing)
                    {
                        IsRefreshing = false;
                    }
                }

                IsFirstLoading = false;
            }
        }
    }
}
