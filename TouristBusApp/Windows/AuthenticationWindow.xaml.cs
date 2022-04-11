using System;
using System.Windows;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.Windows
{
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = new LoginWindow();
            window.Owner = this;
            if (window.ShowDialog() != true) return;
            
            if (ProjectResource.Instance.Authentication.GetSignedInUser().Role == UserRole.Admin)
            {
                Application.Current.MainWindow = new AdminUserWindow();
                Application.Current.MainWindow.Show();
                Close();
            }
            else
            {
                Application.Current.MainWindow = new NormalUserWindow();
                Application.Current.MainWindow.Show();
                Close();
            }
        }

        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = new RegisterWindow();
            window.Owner = this;
            if (window.ShowDialog() != true) return;
            
            if (ProjectResource.Instance.Authentication.GetSignedInUser().Role == UserRole.Admin)
            {
                Application.Current.MainWindow = new AdminUserWindow();
                Application.Current.MainWindow.Show();
                Close();
            }
            else
            {
                Application.Current.MainWindow = new NormalUserWindow();
                Application.Current.MainWindow.Show();
                Close();
            }
        }
    }
}