﻿using DIPLOM.Model;
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
        List<int> RequestedAutoParts;
        List<AutoPart> SelectedAutoParts;
        List<Group> SelectedGroups;
        public List<int> BestCombination;
        private Random rnd = new Random();
        private string SelectedCompatibilityName;
        private double FitnessFunctionLimit;
        private double PriceP;
        private double WeightP;
        private double CapacityP;
        private int allowedIndex = 0;
        private static int PopulationCapacity = 10;


        

        public GeneticAlgorithm(DB_Context db, double fitnessFunctionLimit, 
            double priceP, double weightP, double capacityP, 
            string selectedCompatibility, List<Group> selectedGroups, List<AutoPart> requestedParts)
        {
            DB = db;
            PriceP = priceP;
            WeightP = weightP;
            CapacityP = capacityP;
            SelectedCompatibilityName = selectedCompatibility;
            FitnessFunctionLimit = fitnessFunctionLimit;
            SelectedGroups = selectedGroups;
            RequestedAutoParts = new List<int>();
            if (requestedParts != null)
            {
                foreach (AutoPart part in requestedParts)
                {
                    RequestedAutoParts.Add(part.Id);
                }
                allowedIndex = RequestedAutoParts.Count;
            }

        }


        public void GetFitFunctionAll(Population population)
        {
            population.fitFunctions.Clear();
            List<double> fitFunctionsCalc = new List<double>();

            for (int i = 0; i < PopulationCapacity; i++)
            {
                population.fitFunctions.Add(GetFitFunctionOfOne(population.PopulationList[i]));
            }
        }

        private double GetFitFunctionOfOne(Individual individual)
        {
            double function = 0;
            double sumP = 0;
            double sumW = 0;
            double sumC = 0;
            AutoPart part = new AutoPart();
            foreach (int gene in individual.Chromosome)
            {
                part = DB.AutoParts.Where(p => p.Id.Equals(gene)).FirstOrDefault();
                sumP += part.Price;
                sumW += part.Weight;
                sumC += Checkers.CapacityCalc(part.Proportions);
            }

            double sumOfDeparture = 0;

            double priceDeparture = PriceP == 0 ? 0 : Math.Abs((PriceP) - sumP);
            double weightDeparture = WeightP == 0 ? 0 : Math.Abs((WeightP) - sumW);
            double capacityDeparture = CapacityP == 0 ? 0 : Math.Abs((CapacityP) - sumC);

            sumOfDeparture = Math.Pow(priceDeparture + weightDeparture + capacityDeparture, 2);
            function += Math.Sqrt(sumOfDeparture / Math.Sign(PriceP) + Math.Sign(WeightP) + Math.Sign(CapacityP));

            return function;
        }


        public void FindOptimalCombination(BackgroundWorker worker)
        {
            List<string> GroupsNames = new List<string>();
            foreach (Group gr in SelectedGroups)
            {
                GroupsNames.Add(gr.Name);
            }


            List<AutoPart> TempList = DB.AutoParts.Where(a => GroupsNames.Contains(a.GroupName) && a.Amount>0).ToList();

            SelectedAutoParts = new List<AutoPart>();
            if (SelectedCompatibilityName != "ОБЩЕЕ") {
                foreach (AutoPart autoPart in TempList)
                {
                    foreach (Compatibility compatibility in autoPart.Compatibilities)
                    {
                        if (compatibility.Name == SelectedCompatibilityName)
                        {
                            SelectedAutoParts.Add(autoPart);
                        }
                    }
                }
            }
            else
            {
                SelectedAutoParts = TempList;
            }

            if (SelectedAutoParts.Count == 0)
            {
                return;
            }
            Population currentPopulation = new Population();
            for (int i = 0; i < PopulationCapacity; i++)
            {
                int iter = 0;
                iter = rnd.Next(1, 16);
                Individual newIndividual;
                newIndividual = null;
                newIndividual = new Individual(new List<int>());
                if (RequestedAutoParts != null)
                {
                    newIndividual.Chromosome = RequestedAutoParts;
                }
                for (int j = 0; j < iter; j++)
                {
                    newIndividual.Chromosome.Add(SelectedAutoParts.ElementAt(rnd.Next(0, SelectedAutoParts.Count)).Id);
                }
                currentPopulation.PopulationList.Add(newIndividual);
            }

            int populationCount = 0;



            //ОСНОВНОЙ ЦИКЛ АЛГОРИТМА
            while (populationCount < 100 && BestCombination == null)
            {
                if (worker.CancellationPending)
                {
                    return;
                }
                Console.WriteLine("\nПоколение " + populationCount);
                GetFitFunctionAll(currentPopulation);
                Console.WriteLine("Фит функции:");
                for (int i = 0; i < currentPopulation.fitFunctions.Count; i++)
                {
                    Console.Write(i+": "+currentPopulation.fitFunctions[i]+"; ");
                }
                Individual bestOfPopulation = BestIndividual(currentPopulation);
                if (bestOfPopulation != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Лучшая особь:\n");
                    for (int i =0; i< bestOfPopulation.Chromosome.Count; i++)
                    {
                        Console.Write(bestOfPopulation.Chromosome[i] + "; ");
                    }
                    BestCombination = bestOfPopulation.Chromosome;
                }

                Population newPopulation = new Population();

                while (newPopulation.PopulationList.Count < currentPopulation.PopulationList.Count)
                {
                    List<Individual> parents = currentPopulation.RouletteСoupling();
                    Individual child = parents[0].Crossing(parents[1], allowedIndex);
                    if (rnd.Next(0, 100) < 15)
                    {
                        Mutation(rnd.Next(allowedIndex, child.Chromosome.Count), child);
                    }
                    newPopulation.PopulationList.Add(child);
                }

                currentPopulation = newPopulation;

                populationCount++;
            }
        }


        private void Mutation(int index, Individual individual)
        {
            if (index < individual.Chromosome.Count) {
                individual.Chromosome[index] = SelectedAutoParts.ElementAt(rnd.Next(0, SelectedAutoParts.Count)).Id;
            }
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
