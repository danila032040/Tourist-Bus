using System.Windows;

namespace TouristBusApp.Windows
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = new LoginWindow();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Signed in!!!");
            }
        }

        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = new RegisterWindow();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Registered!!!");
            }
        }
    }
}