using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для AutopartWindow.xaml
    /// </summary>
    public partial class AutopartWindow : Window
    {
        DB_Context DB;
        public AutopartWindow(DB_Context db)
        {
            InitializeComponent();
            DB = db;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DB.AutoParts.Load();
            dataGridParts.ItemsSource = DB.AutoParts.Local.ToList();
            //dataGridParts.ItemsSource = 

        }
    }
}
