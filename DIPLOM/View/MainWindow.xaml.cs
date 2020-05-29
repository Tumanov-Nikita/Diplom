using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Windows;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DB_Context DB;
        public MainWindow(DB_Context db)
        {
            InitializeComponent();
            DB = db;

            DB.Compatibilities.Load();
            ObservableCollection<Compatibility> Items = new ObservableCollection<Compatibility>();
            List<Compatibility> compatibilities = DB.Compatibilities.Local.ToList();
            compatibilities = compatibilities.Distinct(new CompatibilityComparer()).ToList();
            
            foreach (Compatibility comp in compatibilities)
            {
                if (Items.IndexOf(comp) == -1)
                {
                    Items.Add(comp);
                }
            }

            ComboBoxCompatibility.ItemsSource = Items;
            ComboBoxCompatibility.SelectedIndex = 0;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //DB.Groups.Load();
            //DB.AutoParts.Load();
            //dataGridNeededParts.ItemsSource = DB.Groups.Local;
            //dataGridNeededParts.Columns[0].Visibility = Visibility.Hidden;
            //foreach (DataGridCell cell in dataGridNeededParts.)
            //DB.AutoParts.Local
       }

        private void MenuItemAutoParts_Click(object sender, RoutedEventArgs e)
        {
            AutopartWindow autopartWindow = new AutopartWindow(DB);
            autopartWindow.Show();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (!Checkers.ParametersValidation(textBoxPrice.Text, textBoxWeight.Text, textBoxCapacity.Text))
            {
                MessageBox.Show("Заполните хотя бы один параметр", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Checkers.PriceValidation(textBoxPrice.Text) &&
                    Checkers.WeightValidation(textBoxWeight.Text) &&
                    Checkers.CapacityValidation(textBoxCapacity.Text))
                {
                    double price =  textBoxPrice.Text == "" ? 0 : Convert.ToDouble(textBoxPrice.Text);
                    double weight = textBoxWeight.Text == "" ? 0 : Convert.ToDouble(textBoxWeight.Text); 
                    double capacity = textBoxCapacity.Text == "" ? 0 : Checkers.CapacityCalc(textBoxCapacity.Text);
                    Compatibility comp = (Compatibility)ComboBoxCompatibility.SelectedValue;


                    GeneticAlgorithmProgress formGeneticAlgorithm = 
                        new GeneticAlgorithmProgress(DB, comp.Name.ToString(), 100000, price, weight, capacity);


                    formGeneticAlgorithm.Show();
                    List<int> res = formGeneticAlgorithm.Result();
                    if (res != null)
                    {
                        dataGridParts.ItemsSource = res;
                    }
                }
            }
        }
    }
}
