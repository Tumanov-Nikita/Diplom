using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для GeneticAlgorithmProgress.xaml
    /// </summary>
    public partial class GeneticAlgorithmProgress : Window
    {
        private DB_Context DB;
        private string SelectedCompatibility;
        private int FitnessFunctionLimit;
        private double PriceP;
        private double WeightP;
        private double CapacityP;

        public GeneticAlgorithmProgress(DB_Context db, string selectedCompatibility, 
            int fitnessFunctionLimit, double priceP, double weightP, double capacityP)
        {
            InitializeComponent();
            DB = db;
            SelectedCompatibility = selectedCompatibility;
            FitnessFunctionLimit = fitnessFunctionLimit;
            PriceP = priceP;
            WeightP = weightP;
            CapacityP = capacityP;
        }

        public List<int> Result()
        {
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(DB, FitnessFunctionLimit);
            List<int> comb = geneticAlgorithm.FindOptimalCombination(PriceP, WeightP, CapacityP, SelectedCompatibility);
            return comb;
        }

    }
}
