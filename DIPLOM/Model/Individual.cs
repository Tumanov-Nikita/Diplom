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

        public Individual Crossing(Individual SecondIndividual)
        {
            int mid = this.Chromosome.Count / 2;
            List<int> NewChromosome = new List<int>();
            for (int i = 0; i < mid; i++)
            {
                NewChromosome.Add(this.Chromosome[i]);
            }
            for (int i = mid; i < SecondIndividual.Chromosome.Count; i++)
            {
                NewChromosome.Add(SecondIndividual.Chromosome[i]);
            }
            Individual ResultInd = new Individual(NewChromosome);
            return ResultInd;
        }

    }
}
