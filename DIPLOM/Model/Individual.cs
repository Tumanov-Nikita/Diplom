using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    class Individual
    {
        Random rnd = new Random();
        public List<int> Chromosome { get; set; }

        public Individual(List<int> chromosome)
        {
            Chromosome = chromosome;
        }

        public Individual Crossing(Individual SecondIndividual, int startInd)
        {
            List<int> NewChromosome = new List<int>();

            if (startInd != 0)
            {
                for (int i = 0; i < startInd; i++)
                {
                    NewChromosome.Add(Chromosome[i]);
                }
            }
            int mid = (this.Chromosome.Count + startInd) / 2;
            for (int i = startInd; i < mid; i++)
            {
                NewChromosome.Add(this.Chromosome[i]);
            }
            for (int i = mid + startInd; i < SecondIndividual.Chromosome.Count; i++)
            {
                NewChromosome.Add(SecondIndividual.Chromosome[i]);
            }
            Individual ResultInd = new Individual(NewChromosome);
            return ResultInd;
        }

    }
}
