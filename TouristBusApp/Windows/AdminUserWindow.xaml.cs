using System.Windows;
using TouristBusApp.Resources;

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
    }
}