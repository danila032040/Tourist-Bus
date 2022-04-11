using System;
using System.Windows;
using TouristBusApp.Resources;

namespace TouristBusApp.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxLogin.Text == "" || PasswordBox.Password == "")
                    throw new Exception("Логин и пароль не должны быть пустыми!!!");
                ProjectResource.Instance.Authentication.SignIn(TextBoxLogin.Text, PasswordBox.Password);
                DialogResult = true;
                Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}