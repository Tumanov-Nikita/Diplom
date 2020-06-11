using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        DB_Context DB;
        public AdminWindow(DB_Context db)
        {
            InitializeComponent();
            DB = db;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DB.Users.Load();
            dataGridUsers.ItemsSource = DB.Users.Local;
            dataGridUsers.Columns[0].Visibility = Visibility.Hidden;
            dataGridUsers.Columns[2].Visibility = Visibility.Hidden;
            dataGridUsers.Columns[1].Width = DataGridLength.Auto;
        }

        private void ButtonUpdDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult questionResult = MessageBox.Show("Эта операция может занимать до 30 минут. Вы уверены?",
                "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (questionResult)
            {
                case MessageBoxResult.Yes:
                    Parsing parsing = new Parsing(DB);
                    parsing.Parse();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }

        }

        private void MenuItemAutoParts_Click(object sender, RoutedEventArgs e)
        {
            AutopartWindow autopartWindow = new AutopartWindow(DB);
            autopartWindow.Show();
        }
    }
}
