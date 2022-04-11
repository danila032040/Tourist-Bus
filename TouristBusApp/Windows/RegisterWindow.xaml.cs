using System;
using System.Windows;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.Windows
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxLogin.Text == "" || PasswordBox.Password == "")
                    throw new Exception("Логин и пароль не должны быть пустыми!!!");
                ProjectResource.Instance.UsersRep.Create(new User
                {
                    Login = TextBoxLogin.Text,
                    Password = PasswordBox.Password,
                    Role = UserRole.Normal
                });
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