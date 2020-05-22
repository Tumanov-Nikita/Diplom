using DIPLOM.Controller;
using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System.Windows;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        static DB_Context DB;
        UserController userController;
        public RegistrationWindow(DB_Context db)
        {
            InitializeComponent();
            DB = db;
            userController = new UserController(DB);
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == txtPassword2.Password) {
                userController.Add(textBoxLogin.Text, txtPassword2.Password, Role.User);
                MessageBox.Show("Пользователь успешно создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
