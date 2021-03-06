﻿using DIPLOM.Infrastructure;
using DIPLOM.Model;
using Microsoft.Win32;
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
        ObservableCollection<AutopartView> PartList;
        List<AutoPart> ResultParts;

        public MainWindow(DB_Context db)
        {
            InitializeComponent();
            DB = db;

            try
            {
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
                    if (!Items.Where(i => i.Name == comp.Name).Any())
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
                    GroupList.Add(new GroupView() { Group = currGroup, _isChecked = true });
                }
                RequestedGroups.ItemsSource = GroupList;

                DB.AutoParts.Load();
                PartList = new ObservableCollection<AutopartView>();

                foreach (AutoPart currAutoPart in DB.AutoParts.Local.Where(a=>a.Amount>0).ToList())
                {
                    PartList.Add(new AutopartView(currAutoPart, false));
                }
                RequestedParts.ItemsSource = PartList;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось отобрать компоненты для заданных параметров\n"+ex.Message, "Подбор неудачен", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            AlgorithmInput input = (AlgorithmInput)e.Argument;

            e.Result = StartAlgorithm(worker, input.Price, input.Weight, input.Capacity, input.SelectedValue, input.SelectedGroups, input.RequestedParts);

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
                ResultParts = new List<AutoPart>();
                foreach (int id in ResultIDs)
                {
                    AutoPart part = DB.AutoParts.Where(p => p.Id == id).FirstOrDefault();
                    ResultParts.Add(part);
                }
                dataGridParts.ItemsSource = ResultParts;
                buttonOK.IsEnabled = true;
                buttonSaveReport.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Не удалось отобрать компоненты по заданным данных", "Подбор неудачен", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void MenuItemAutoParts_Click(object sender, RoutedEventArgs e)
        {
            AutopartWindow autopartWindow = new AutopartWindow(DB);
            autopartWindow.Show();
        }

        private List<int> StartAlgorithm(BackgroundWorker backgroundWorker, string textPrice, 
            string textWeight, string textCapacity, string selectedValue, 
            List<Group> selectedGroups, List<AutoPart> requestedParts)
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

                    // Настройка допустимого отклонения
                    double limit = (price + weight + capacity) / 10;

                    GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(DB, limit, 
                        price, weight, capacity, 
                        selectedValue, selectedGroups, requestedParts);


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

                List<AutoPart> selectedAutoParts = new List<AutoPart>();
                foreach (AutopartView autopartView in PartList)
                {
                    if (autopartView._isChecked)
                    {
                        selectedAutoParts.Add(autopartView.AutoPart);
                    }
                }

                progressBar.Visibility = Visibility.Visible;
                buttonCancel.IsEnabled = true;
                buttonSaveReport.IsEnabled = false;


                AlgorithmInput input = new AlgorithmInput(textBoxPrice.Text, textBoxWeight.Text, textBoxCapacity.Text, 
                    ComboBoxCompatibility.Text, selectedGroups, selectedAutoParts);
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

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            foreach(AutoPart autoPart in ResultParts)
            {
                var EditedValue = DB.AutoParts.Where(c => c.Id == autoPart.Id)
                            .FirstOrDefault();
                EditedValue.Amount -= 1;
                DB.SaveChanges();
            }
            buttonOK.IsEnabled = false;
        }

        private void ButtonSaveReport_Click(object sender, RoutedEventArgs e)
        {
            if (ResultParts != null)
            {
                SavingReport saving = new SavingReport();
                string fileName = String.Empty;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == true)
                {
                    fileName = saveFileDialog.FileName;
                }
                else
                    return;

                saving.SaveInExcelReport(ResultParts, fileName);
            }
        }

        private void ComboBoxCompatibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PartList = null;
            PartList = new ObservableCollection<AutopartView>();
            Compatibility selectedComp = (Compatibility)ComboBoxCompatibility.SelectedItem;
            if (selectedComp.Name == "ОБЩЕЕ")
            { 
                foreach (AutoPart autoPart in DB.AutoParts.Local)
                {
                    PartList.Add(new AutopartView(autoPart, false));
                }
            }
            else
            {
                //ObservableCollection<AutopartView> CurrentParts = null;
                //CurrentParts = new ObservableCollection<AutopartView>();
                foreach (AutoPart autoPart in DB.AutoParts.Local)
                {
                    foreach (Compatibility compatibility in autoPart.Compatibilities)
                    {
                        if (compatibility.Name == selectedComp.Name)
                        {
                            PartList.Add(new AutopartView(autoPart, false));
                        }
                    }
                }
                //PartList = CurrentParts;
            }
            RequestedParts.ItemsSource = PartList;
        }
    }


    public class AlgorithmInput
    {
        public string Price { get; set; }

        public string Weight { get; set; }

        public string Capacity { get; set; }

        public string SelectedValue { get; set; }

        public List<Group> SelectedGroups { get; set; }

        public List<AutoPart> RequestedParts { get; set; }

        public AlgorithmInput(string textPrice, string textWeight, string textCapacity, 
            string selectedCompatibility, List<Group> selectedGroups, List<AutoPart> requestedParts)
        {
            Price = textPrice;
            Weight = textWeight;
            Capacity = textCapacity;
            SelectedValue = selectedCompatibility;
            SelectedGroups = selectedGroups;
            RequestedParts = requestedParts;
        }

    }
}
