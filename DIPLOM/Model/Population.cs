using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    class Population
    {
        Random rnd = new Random();
        public List<double> fitFunctions;
        public List<Individual> PopulationList { get; set; }
        //private int AmountOfProperties;
        //private int FitnessFunctionLimit;


        public Population()
        {
            PopulationList = new List<Individual>();
            fitFunctions = new List<double>();
        }

        
        public List<Individual> RouletteСoupling()
        {
            if (fitFunctions != null)
            {
                Individual parent1 = null;
                Individual parent2 = null;
                double sum = 0;
                List<double> probabilities = new List<double>();

                sum = fitFunctions.Sum();

                foreach (double elem in fitFunctions)
                {
                    probabilities.Add(elem / sum);
                }

                while (parent2 == null)
                {
                    int expectantIndex = rnd.Next(fitFunctions.Count);
                    if (probabilities[expectantIndex] < rnd.Next(100))
                    {
                        if (parent1 == null)
                        {
                            parent1 = PopulationList[expectantIndex];
                        }
                        else
                        {
                            if (parent1 != PopulationList[expectantIndex])
                            {
                                parent2 = PopulationList[expectantIndex];
                            }
                        }
                    }
                }

                List<Individual> Parents = new List<Individual>();
                Parents.Add(parent1);
                Parents.Add(parent2);
                return Parents;
            }
            else
            {
                return null;
            }
        }

    }
}
