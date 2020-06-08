using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DB_Context DB;
        BackgroundWorker worker;
        ObservableCollection<GroupView> GroupList;

        public MainWindow(DB_Context db)
        {
            InitializeComponent();
            DB = db;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            DB.Compatibilities.Load();
            ObservableCollection<Compatibility> Items = new ObservableCollection<Compatibility>();
            List<Compatibility> compatibilities = DB.Compatibilities.Local.ToList();
            compatibilities = compatibilities.Distinct(new CompatibilityComparer()).ToList();
            
            foreach (Compatibility comp in compatibilities)
            {
                if (!Items.Where(i=>i.Name == comp.Name).Any())
                {
                    Items.Add(comp);
                }
            }

            ComboBoxCompatibility.ItemsSource = Items;
            ComboBoxCompatibility.SelectedIndex = 0;

            DB.Groups.Load();

            GroupList = new ObservableCollection<GroupView>();
            foreach (Group currGroup in DB.Groups.Local.ToList())
            {
                GroupList.Add(new GroupView() {  Group = currGroup, _isChecked = true});
            }

            RequestedParts.ItemsSource = GroupList;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            AlgorithmInput input = (AlgorithmInput)e.Argument;

            e.Result = StartAlgorithm(worker, input.Price, input.Weight, input.Capacity, input.SelectedValue, input.SelectedGroups);

            if (worker.CancellationPending == true)
            {
                MessageBox.Show("Операция прервана пользователем", "", MessageBoxButton.OK, MessageBoxImage.Information);
                e.Result = null;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Произошла ошибка подбора");
            }

            progressBar.Visibility = Visibility.Hidden;
            buttonCancel.IsEnabled = false;

            if (e.Result != null)
            {
                List<int> ResultIDs = (List<int>)e.Result;
                List<AutoPart> ResultParts = new List<AutoPart>();
                foreach (int id in ResultIDs)
                {
                    AutoPart part = DB.AutoParts.Where(p => p.Id == id).FirstOrDefault();
                    ResultParts.Add(part);
                }
                dataGridParts.ItemsSource = ResultParts;
            }
            else
            {
                MessageBox.Show("Не удалось отобрать компоненты для заданных параметров", "Подбор неудачен", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void MenuItemAutoParts_Click(object sender, RoutedEventArgs e)
        {
            AutopartWindow autopartWindow = new AutopartWindow(DB);
            autopartWindow.Show();
        }

        private List<int> StartAlgorithm(BackgroundWorker backgroundWorker, string textPrice, string textWeight, string textCapacity, string selectedValue, List<Group> selectedGroups)
        {
            if (!Checkers.ParametersValidation(textPrice, textWeight, textCapacity))
            {
                MessageBox.Show("Заполните хотя бы один параметр", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Checkers.PriceValidation(textPrice) &&
                    Checkers.WeightValidation(textWeight) &&
                    Checkers.CapacityValidation(textCapacity))
                {
                    double price = textPrice == "" ? 0 : Convert.ToDouble(textPrice);
                    double weight = textWeight == "" ? 0 : Convert.ToDouble(textWeight);
                    double capacity = textCapacity == "" ? 0 : Checkers.CapacityCalc(textCapacity);

                    double limit = (price + weight + capacity) / 10;

                    GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(DB, limit, price, weight, capacity, selectedValue, selectedGroups);


                    geneticAlgorithm.FindOptimalCombination(worker);

                    return geneticAlgorithm.BestCombination;
                }
            }
                return null;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {

            if (Checkers.CheckSelectedGroups(GroupList))
            {
                List<Group> selectedGroups = new List<Group>();

                foreach (GroupView groupView in GroupList)
                {
                    if (groupView._isChecked)
                    {
                        selectedGroups.Add(groupView.Group);
                    }
                }

                progressBar.Visibility = Visibility.Visible;
                buttonCancel.IsEnabled = true;


                AlgorithmInput input = new AlgorithmInput(textBoxPrice.Text, textBoxWeight.Text, textBoxCapacity.Text, ComboBoxCompatibility.Text, selectedGroups);
                try
                {
                    worker.RunWorkerAsync(input);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка подбора компонентов\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }
    }


    public class AlgorithmInput
    {
        public string Price { get; set; }

        public string Weight { get; set; }

        public string Capacity { get; set; }

        public string SelectedValue { get; set; }

        public List<Group> SelectedGroups { get; set; }

        public AlgorithmInput(string textPrice, string textWeight, string textCapacity, string selectedCompatibility, List<Group> selectedGroups)
        {
            Price = textPrice;
            Weight = textWeight;
            Capacity = textCapacity;
            SelectedValue = selectedCompatibility;
            SelectedGroups = selectedGroups;
        }

    }
}
