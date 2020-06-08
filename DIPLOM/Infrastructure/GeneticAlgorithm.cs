using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DIPLOM.Infrastructure
{
    class GeneticAlgorithm
    {
        private DB_Context DB;
        List<AutoPart> SelectedAutoParts;
        List<Group> SelectedGroups;
        public List<int> BestCombination;
        private Random rnd = new Random();
        private string SelectedCompatibilityName;
        private int FitnessFunctionLimit;
        private double PriceP;
        private double WeightP;
        private double CapacityP;
        private static int PopulationCapacity = 10;

        

        public GeneticAlgorithm(DB_Context db, int fitnessFunctionLimit, double priceP, double weightP, double capacityP, string selectedCompatibility, List<Group> selectedGroups)
        {
            DB = db;
            PriceP = priceP;
            WeightP = weightP;
            CapacityP = capacityP;
            SelectedCompatibilityName = selectedCompatibility;
            FitnessFunctionLimit = fitnessFunctionLimit;
            SelectedGroups = selectedGroups;
        }


        public void GetFitFunctionAll(Population population)
        {
            population.fitFunctions.Clear();
            List<double> fitFunctionsCalc = new List<double>();
            //Task<double>[] individualsFunctions = new Task<double>[PopulationCapacity];
            //for (int i = 0; i < PopulationCapacity; i++)
            //{
            //    individualsFunctions[i] = new Task<double>(() => GetFitFunctionOfOne(population.PopulationList[i-1]));
            //    individualsFunctions[i].Start();
            //}
            //Task.WaitAll(individualsFunctions);
            //foreach(Task<double> t in individualsFunctions)
            //{
            //    population.fitFunctions.Add(t.Result);
            //}
            for (int i = 0; i < PopulationCapacity; i++)
            {
                population.fitFunctions.Add(GetFitFunctionOfOne(population.PopulationList[i]));
            }
        }

        private double GetFitFunctionOfOne(Individual individual)
        {
            double function = 0;
            AutoPart part = new AutoPart();
            foreach (int gene in individual.Chromosome)
            {
                double sumOfDeparture = 0;

                part = DB.AutoParts.Where(p => p.Id.Equals(gene)).FirstOrDefault();
                double priceDeparture = PriceP == 0 ? 0 : Math.Abs((PriceP) - part.Price);
                double weightDeparture = WeightP == 0 ? 0 : Math.Abs((WeightP) - part.Weight);
                double capacityDeparture = CapacityP == 0 ? 0 : Math.Abs((CapacityP) - Checkers.CapacityCalc(part.Proportions));

                sumOfDeparture = Math.Pow(priceDeparture + weightDeparture + capacityDeparture, 2);
                function += Math.Sqrt(sumOfDeparture/Math.Sign(PriceP) + Math.Sign(WeightP) + Math.Sign(CapacityP));
            }
            return function;
        }


        public void FindOptimalCombination(BackgroundWorker worker)
        {
            Compatibility selectCompatibility = (Compatibility)DB.Compatibilities.Where(c => c.Name == SelectedCompatibilityName).FirstOrDefault();
            List<string> GroupsNames = new List<string>();
            foreach (Group gr in SelectedGroups)
            {
                GroupsNames.Add(gr.Name);
            }

            SelectedAutoParts = DB.AutoParts.Where(a => GroupsNames.Contains(a.GroupName)).ToList();
            foreach (AutoPart part in SelectedAutoParts)
            {
                if (part.Compatibilities.Count == 0 && part.CompatibilitiesNames.Count == 0)
                {
                    Compatibility Common = DB.Compatibilities.Where(c => c.Name == "ОБЩЕЕ").FirstOrDefault();
                    part.Compatibilities.Add(Common);
                    part.CompatibilitiesNames.Add("ОБЩЕЕ");
                }
            }
            SelectedAutoParts = SelectedAutoParts.Where(s=>s.CompatibilitiesNames.Contains(selectCompatibility.Name)).ToList();

            Population currentPopulation = new Population();
            for (int i = 0; i < PopulationCapacity; i++)
            {
                int iter = rnd.Next(5,16);
                List<int> chromosomes = new List<int>();
                for (int j = 0; j < iter; j++)
                {
                    chromosomes.Add(SelectedAutoParts.ElementAt(rnd.Next(0, SelectedAutoParts.Count)).Id);
                }
                currentPopulation.PopulationList.Add(new Individual(chromosomes));
            }

            int populationCount = 0;

            while (populationCount < 100)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                GetFitFunctionAll(currentPopulation);
                Individual bestOfPopulation = BestIndividual(currentPopulation);
                if (bestOfPopulation != null)
                {
                    BestCombination = bestOfPopulation.Chromosome;
                    //return bestOfPopulation.Chromosome;
                }

                Population newPopulation = new Population();

                while (newPopulation.PopulationList.Count < currentPopulation.PopulationList.Count)
                {
                    List<Individual> parents = currentPopulation.RouletteСoupling();
                    Individual child = parents[0].Crossing(parents[1]);
                    newPopulation.PopulationList.Add(child);
                }

                currentPopulation = newPopulation;

                Console.WriteLine(populationCount);

                populationCount++;
            }


            //return null;
        }


        private void Mutation(int index, Individual individual)
        {
            individual.Chromosome[index] = SelectedAutoParts.ElementAt(rnd.Next(0, SelectedAutoParts.Count)).Id;
        }

        private Individual BestIndividual(Population population)
        {
            double bestFit = population.fitFunctions.Min();
            if (bestFit < FitnessFunctionLimit)
            {
                return population.PopulationList[population.fitFunctions.IndexOf(bestFit)];
            }
            else return null;
        }
    }
}
