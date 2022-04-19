using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;
using TouristBusApp.UserControls;

namespace TouristBusApp.Windows
{
    public partial class NormalUserWindow : Window
    {
        private Filter _filter;
        private readonly List<TourPoint> _filteringTourPoints;

        public NormalUserWindow()
        {
            _filteringTourPoints = new List<TourPoint>();

            InitializeComponent();
            RefreshTourStackPanel();
        }

        private int SortOrder
        {
            get => ChangeSortOrderButton.Content.ToString() == "↓" ? 1 : -1;
            set => ChangeSortOrderButton.Content = value == 1 ? "↓" : "↑";
        }

        private void ButtonSignOut_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.Authentication.SignOut();
            Application.Current.MainWindow = new AuthenticationWindow();
            Application.Current.MainWindow.Show();
            Close();
        }

        private void FilterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _filter = (Filter) (sender as ComboBox).SelectedIndex;
            RefreshTourStackPanel();
        }

        private void RefreshTourStackPanel()
        {
            if (TourStackPanel == null) return;
            TourStackPanel.Children.Clear();

            List<Tour> tours = ProjectResource.Instance.ToursRep.Read().ToList();
            switch (_filter)
            {
                case Filter.None:
                    break;
                case Filter.Name:
                    tours.Sort((t1, t2) => SortOrder * string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
                    break;
                case Filter.Departure:
                    tours.Sort((t1, t2) => SortOrder * t1.Departure.CompareTo(t2.Departure));
                    break;
                case Filter.Arrival:
                    tours.Sort((t1, t2) => SortOrder * t1.Arrival.CompareTo(t2.Arrival));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Tour tour in tours.Where(t =>
                             _filteringTourPoints.Count == 0 || _filteringTourPoints.All(tp => t.TourPointIds.Contains(tp.Id))))
                TourStackPanel.Children.Add(new TourControl(tour));
        }

        private void ChangeSortOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            SortOrder = -SortOrder;
            RefreshTourStackPanel();
        }

        private enum Filter
        {
            None,
            Name,
            Departure,
            Arrival
        }

        private void FilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AddTourPointFilterWindow();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                if (!_filteringTourPoints.Any(tp => tp.Equals(window.TourPoint)))
                    _filteringTourPoints.Add(window.TourPoint);
                RefreshFilters();
            }
        }

        private void RefreshFilters()
        {
            FiltersStackPanel.Children.Clear();

            foreach (TourPoint tp in _filteringTourPoints)
            {
                var button = new Button()
                {
                    Content = tp.Name,
                    DataContext = tp,
                    ToolTip = "Удалить",
                    Margin = new Thickness(5),
                };
                button.Click += DeleteFilterButton_OnClick;
                FiltersStackPanel.Children.Add(button);
            }

            RefreshTourStackPanel();
        }

        private void DeleteFilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            _filteringTourPoints.Remove((sender as Button).DataContext as TourPoint);
            RefreshFilters();
        }
    }
}