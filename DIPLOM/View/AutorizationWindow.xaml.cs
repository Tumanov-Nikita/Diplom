using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        static DB_Context DB;
        public AutorizationWindow()
        {
            InitializeComponent();
            DB = new DB_Context();        }

        private void ButtonAutorization_Click(object sender, RoutedEventArgs e)
        {
            if (Checkers.Firmness(textBoxLogin.Text) && Checkers.Firmness(txtPassword.Password))
            {
                try
                {
                    string passHash = Encrypting.ComputeHash(txtPassword.Password);
                    var user = DB.Users.Where(l => l.Login.Equals(textBoxLogin.Text) && l.PassHash.Equals(passHash));
                    if (user.Any())
                    {
                        if (user.FirstOrDefault().Role == Role.Admin)
                        {
                            AdminWindow adminWindow = new AdminWindow(DB);
                            this.Visibility = Visibility.Hidden;
                            adminWindow.ShowDialog();
                            this.Visibility = Visibility.Visible;
                        } 
                        else if (user.FirstOrDefault().Role == Role.User)
                        {
                            MainWindow mainWindow = new MainWindow(DB);
                            this.Visibility = Visibility.Hidden;
                            mainWindow.ShowDialog();
                            this.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неправильно введен логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка авторизации\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow(DB);
            this.IsEnabled = false;
            registration.ShowDialog();
            this.IsEnabled = true;
        }
    }
}
