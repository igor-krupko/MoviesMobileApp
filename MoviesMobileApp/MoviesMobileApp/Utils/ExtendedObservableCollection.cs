using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MoviesMobileApp.Utils
{
    public sealed class ExtendedObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";
        
        [SuppressMessage("Reliability",
            "S2743:Static fields should not be used in generic types",
            Justification = "Legacy code")]
        private static readonly NotifyCollectionChangedEventHandler EmptyDelegate = delegate { };
        
        private readonly ReentryMonitor monitor = new ReentryMonitor();
        private readonly NotificationInfo notifyInfo;
        
        private bool disableReentry;
        private Action fireCountAndIndexerChanged = delegate { };
        private Action fireIndexerChanged = delegate { };
        
        private event PropertyChangedEventHandler PropertyChanged;
        private event NotifyCollectionChangedEventHandler CollectionChanged = EmptyDelegate;

        public ExtendedObservableCollection(List<T> list)
            : base(new List<T>(list.Count))
        {
            list.ForEach(Items.Add);
        }

        public ExtendedObservableCollection(ExtendedObservableCollection<T> parent, bool notify)
            : base(parent.Items)
        {
            notifyInfo = new NotificationInfo
            {
                RootCollection = parent
            };

            if (notify)
            {
                CollectionChanged = notifyInfo.Initialize();
            }
        }

        ~ExtendedObservableCollection()
        {
            DisposeInternal();
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                if (notifyInfo == null)
                {
                    if (PropertyChanged == null)
                    {
                        fireCountAndIndexerChanged = delegate
                        {
                            OnPropertyChanged(new PropertyChangedEventArgs(CountString));
                            OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
                        };
                        fireIndexerChanged = delegate { OnPropertyChanged(new PropertyChangedEventArgs(IndexerName)); };
                    }

                    PropertyChanged += value;
                }
                else
                {
                    notifyInfo.RootCollection.PropertyChanged += value;
                }
            }

            remove
            {
                if (notifyInfo == null)
                {
                    PropertyChanged -= value;

                    if (PropertyChanged != null)
                    {
                        return;
                    }

                    fireCountAndIndexerChanged = delegate { };
                    fireIndexerChanged = delegate { };
                }
                else
                {
                    notifyInfo.RootCollection.PropertyChanged -= value;
                }
            }
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                if (notifyInfo == null)
                {
                    if (CollectionChanged.GetInvocationList().Length == 1)
                    {
                        CollectionChanged -= EmptyDelegate;
                    }

                    CollectionChanged += value;
                    disableReentry = CollectionChanged.GetInvocationList().Length > 1;
                }
                else
                {
                    notifyInfo.RootCollection.CollectionChanged += value;
                }
            }

            remove
            {
                if (notifyInfo == null)
                {
                    CollectionChanged -= value;

                    if (CollectionChanged == null || CollectionChanged.GetInvocationList().Length == 0)
                    {
                        CollectionChanged += EmptyDelegate;
                    }

                    disableReentry = CollectionChanged.GetInvocationList().Length > 1;
                }
                else
                {
                    notifyInfo.RootCollection.CollectionChanged -= value;
                }
            }
        }

        public ExtendedObservableCollection<T> DelayNotifications()
        {
            return new ExtendedObservableCollection<T>((null == notifyInfo)
                    ? this
                    : notifyInfo.RootCollection,
                true);
        }

        protected override void ClearItems()
        {
            CheckReentrancy();

            base.ClearItems();

            fireCountAndIndexerChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            var removedItem = this[index];

            base.RemoveItem(index);

            fireCountAndIndexerChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
        }

        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();

            base.InsertItem(index, item);

            fireCountAndIndexerChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();

            var originalItem = this[index];
            base.SetItem(index, item);

            fireIndexerChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                originalItem,
                item,
                index));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            using (BlockReentrancy())
            {
                CollectionChanged(this, eventArgs);
            }
        }

        private IDisposable BlockReentrancy()
        {
            return monitor.Enter();
        }

        private void CheckReentrancy()
        {
            if (disableReentry && monitor.IsNotifying)
            {
                throw new InvalidOperationException("ObservableCollectionEx Reentrancy Not Allowed");
            }
        }

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        private void DisposeInternal()
        {
            if (notifyInfo == null || !notifyInfo.HasEventArgs)
            {
                return;
            }

            if (notifyInfo.RootCollection.PropertyChanged != null)
            {
                if (notifyInfo.IsCountChanged)
                {
                    notifyInfo.RootCollection.OnPropertyChanged(new PropertyChangedEventArgs(CountString));
                }

                notifyInfo.RootCollection.OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            }

            using (notifyInfo.RootCollection.BlockReentrancy())
            {
                var eventArgs = notifyInfo.EventArgs;

                foreach (var delegateItem in notifyInfo.RootCollection.CollectionChanged.GetInvocationList())
                {
                    delegateItem.DynamicInvoke(notifyInfo.RootCollection, eventArgs);
                }
            }
                
            CollectionChanged = notifyInfo.Initialize();
        }

        private class NotificationInfo
        {
            private NotifyCollectionChangedAction? action;
            private IList newItems;
            private IList oldItems;
            private int newIndex;
            private int oldIndex;

            public NotifyCollectionChangedEventHandler Initialize()
            {
                action = null;
                newItems = null;
                oldItems = null;

                return (sender, args) =>
                {
                    var wrapper = sender as ExtendedObservableCollection<T>;
                    if (wrapper == null)
                    {
                        return;
                    }
                    action = args.Action;

                    switch (action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            newItems = new List<T>();
                            IsCountChanged = true;
                            wrapper.CollectionChanged = (s, e) =>
                            {
                                foreach (T item in e.NewItems)
                                {
                                    newItems.Add(item);
                                }
                            };
                            wrapper.CollectionChanged(sender, args);
                            break;

                        case NotifyCollectionChangedAction.Remove:
                            oldItems = new List<T>();
                            IsCountChanged = true;
                            wrapper.CollectionChanged = (s, e) =>
                            {
                                foreach (T item in e.OldItems)
                                {
                                    oldItems.Add(item);
                                }
                            };
                            wrapper.CollectionChanged(sender, args);
                            break;

                        case NotifyCollectionChangedAction.Replace:
                            newItems = new List<T>();
                            oldItems = new List<T>();
                            wrapper.CollectionChanged = (s, e) =>
                            {
                                foreach (T item in e.NewItems)
                                {
                                    newItems.Add(item);
                                }

                                foreach (T item in e.OldItems)
                                {
                                    oldItems.Add(item);
                                }
                            };
                            wrapper.CollectionChanged(sender, args);
                            break;
                        case NotifyCollectionChangedAction.Move:
                            newIndex = args.NewStartingIndex;
                            newItems = args.NewItems;
                            oldIndex = args.OldStartingIndex;
                            oldItems = args.OldItems;
                            wrapper.CollectionChanged = (s, e) => throw new InvalidOperationException("Due to design of NotifyCollectionChangedEventArgs combination of multiple Move operations is not possible");
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            IsCountChanged = true;
                            break;
                    }
                };
            }

            public ExtendedObservableCollection<T> RootCollection { get; set; }

            public bool IsCountChanged { get; private set; }

            public NotifyCollectionChangedEventArgs EventArgs
            {
                get
                {
                    switch (action)
                    {
                        case NotifyCollectionChangedAction.Reset:
                            return new NotifyCollectionChangedEventArgs(action.Value);

                        case NotifyCollectionChangedAction.Add:
                            return new NotifyCollectionChangedEventArgs(action.Value, newItems);

                        case NotifyCollectionChangedAction.Remove:
                            return new NotifyCollectionChangedEventArgs(action.Value, oldItems);

                        case NotifyCollectionChangedAction.Move:
                            return new NotifyCollectionChangedEventArgs(action.Value, oldItems[0], newIndex, oldIndex);

                        case NotifyCollectionChangedAction.Replace:
                            return new NotifyCollectionChangedEventArgs(action.Value, newItems, oldItems);
                    }

                    return null;
                }
            }

            public bool HasEventArgs => action.HasValue;
        }
    }
}