using System.Collections.Specialized;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Collections;
using System.Windows.Media;
using NetChat2.ViewModel;

namespace NetChat2.Source
{
    public static class Helper
    {
        public static T FindChild<T>(this DependencyObject parent)
                where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (!(child is T childType))
                {
                    foundChild = child.FindChild<T>();
                    if (foundChild != null) break;
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }
    }

    public class BringNewItemIntoViewBehavior : Behavior<ItemsControl>
    {
        private INotifyCollectionChanged notifier;
        private ScrollViewer scrollViewer;
        private Button gotoNewMessagesButton;

        protected override void OnAttached()
        {
            base.OnAttached();
            notifier = AssociatedObject.Items as INotifyCollectionChanged;
            notifier.CollectionChanged += ItemsControl_CollectionChanged;


            AssociatedObject.Loaded += (o, e) =>
            {
                scrollViewer = AssociatedObject.FindChild<ScrollViewer>();
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;

                gotoNewMessagesButton = AssociatedObject.FindChild<Button>();
                gotoNewMessagesButton.Click += ShowNewMessagesButton_Click;
                NewMessagesCount = 0;
            };
        }

        private void ShowNewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            GoToIndex(AssociatedObject.Items.Count - NewMessagesCount);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            notifier.CollectionChanged -= ItemsControl_CollectionChanged;
            scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            gotoNewMessagesButton.Click -= ShowNewMessagesButton_Click;
        }

        private void ItemsControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                GoToIndex(AssociatedObject.Items.Count - 1);

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                NewMessagesCount++;
                if (ScrollViewerWasOnBottom())
                {
                    GoToIndex(e.NewStartingIndex);
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0) return;
            LastVisibleIndex = GetLastVisibleElementIndex();
        }

        private void GoToIndex(int index)
        {
            LastVisibleIndex = index;
            var item = (FrameworkElement)AssociatedObject.ItemContainerGenerator.ContainerFromIndex(index);
            item?.BringIntoView();
        }
        private bool ScrollViewerWasOnBottom()
        {
            return LastVisibleIndex == AssociatedObject.Items.Count - 2;
        }

        private int GetLastVisibleElementIndex()
        {
            int lastIndex = 0;
            for (int i = AssociatedObject.Items.Count - 1; i > 0; i--)
            {
                FrameworkElement elm = (FrameworkElement)AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i);
                if (IsVisible(elm, AssociatedObject))
                    lastIndex = Math.Max(lastIndex, i);
            }
            return lastIndex;
        }
        private bool IsVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds =
                element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }


        private int _lastVisibleIndex;
        private int LastVisibleIndex
        {
            get => _lastVisibleIndex;
            set
            {
                _lastVisibleIndex = value;
                if (NewMessagesCount > 0)
                {
                    int firstNewMessageIndex = AssociatedObject.Items.Count - NewMessagesCount;
                    int readedNewMessages = _lastVisibleIndex - firstNewMessageIndex + 1;
                    if (readedNewMessages > 0)
                    {
                        NewMessagesCount -= readedNewMessages;
                        for (int i = firstNewMessageIndex; i <= _lastVisibleIndex; i++)
                            ((IReadable)AssociatedObject.Items[i]).Read();
                    }
                }
            }
        }

        private int _newMessagesCount;
        private int NewMessagesCount
        {
            get => _newMessagesCount;
            set
            {
                _newMessagesCount = value;
                if (gotoNewMessagesButton == null) return;
                gotoNewMessagesButton.Content = value;
                if (_newMessagesCount > 0)
                    gotoNewMessagesButton.Visibility = Visibility.Visible;
                else
                    gotoNewMessagesButton.Visibility = Visibility.Collapsed;
            }
        }

    }
}
