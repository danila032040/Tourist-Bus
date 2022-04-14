using System;
using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;
using TouristBusApp.UserControls;

namespace TouristBusApp.Windows
{
    public partial class AdminUserWindow : Window
    {
        public AdminUserWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignOut_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.Authentication.SignOut();
            Application.Current.MainWindow = new AuthenticationWindow();
            Application.Current.MainWindow.Show();
            Close();
        }

        private void ButtonAddTour_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.ToursRep.Create(new Tour
            {
                Name = "Test",
                Bus = null,
                TourPointIds = null,
                Departure = DateTime.Now,
                Arrival = DateTime.Now.AddDays(1)
            });
            RefreshTourStackPanel();
        }

        private void ButtonAddTourPoint_OnClick(object sender, RoutedEventArgs e)
        {
            AddTourPointWindow window = new AddTourPointWindow();
            if (window.ShowDialog() != true) return;
            
            
            ProjectResource.Instance.TourPointsRep.Create(window.TourPoint);
            RefreshTourPointsPanel();
        }


        private void RefreshTourStackPanel()
        {
            ToursStackPanel.Children.Clear();
            foreach (Tour tour in ProjectResource.Instance.ToursRep.Read())
            {
                Button deleteButton = new Button
                {
                    Content = "X"
                };
                deleteButton.DataContext = tour.Id;
                deleteButton.Click += DeleteTourButton_OnClick;
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 50)
                };
                sp.Children.Add(new EditableTourControl(tour));
                sp.Children.Add(deleteButton);
                ToursStackPanel.Children.Add(sp);
            }
        }

        private void RefreshBusStackPanel()
        {
            BusStackPanel.Children.Clear();
            foreach (Bus bus in ProjectResource.Instance.BussesRep.Read())
            {
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 50)
                };
                sp.Children.Add(new BusControl(bus));
                BusStackPanel.Children.Add(sp);
            }
        }

        private void RefreshTourPointsPanel()
        {
            TourPointsStackPanel.Children.Clear();
            foreach (TourPoint tourPoint in ProjectResource.Instance.TourPointsRep.Read())
            {
                Button deleteButton = new Button
                {
                    Content = "X"
                };
                deleteButton.DataContext = tourPoint.Id;
                deleteButton.Click += DeleteTourPointButton_OnClick;
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 50)
                };
                sp.Children.Add(new EditableTourPointControl(tourPoint));
                sp.Children.Add(deleteButton);
                TourPointsStackPanel.Children.Add(sp);
            }
        }

        

        private void DeleteTourButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.ToursRep.Delete((int) (sender as Button).DataContext);
            RefreshTourStackPanel();
        }
        
        private void DeleteTourPointButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.TourPointsRep.Delete((int) (sender as Button).DataContext);
            RefreshTourPointsPanel();
        }

        private void TourTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshTourStackPanel();
        }

        private void BusTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshBusStackPanel();
        }

        private void TourPointTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshTourPointsPanel();
        }
    }
}