using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;
using TouristBusApp.UserControls;

namespace TouristBusApp.Windows
{
    public partial class NormalUserWindow : Window
    {
        private enum Filter
        {
            None,
            Name,
            Departure,
            Arrival
        }

        private Filter _filter;
        public NormalUserWindow()
        {
            InitializeComponent();
            
            RefreshTourStackPanel();
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
                
            foreach (Tour tour in ProjectResource.Instance.ToursRep.Read())
            {
                TourStackPanel.Children.Add(new TourControl(tour));
            }
            
        }
    }
}