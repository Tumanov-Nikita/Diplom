using DIPLOM.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class AutoPart 
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public List<Compatibility> Compatibilities { get; set; }
        public double Price { get; set; }
        public string Proportions { get; set; }
        public double Weight { get; set; }
        public double Amount { get; set; }


        public AutoPart(string article, string name, string groupName, string subGroupName, 
                        double price, string proportions, double amount, double weight)
        {
            Article = article;
            Name = name;
            GroupName = groupName;
            SubGroupName = subGroupName;
            Price = price;
            Proportions = proportions;
            Amount = amount;
            Weight = weight;
            Compatibilities = new List<Compatibility>();
        }

        public AutoPart()
        {
        }

        public void AddCompatibility(Compatibility ownerShip)
        {
            if (!Compatibilities.Contains(ownerShip))
            {
                Compatibilities.Add(ownerShip);
            }
        }

        public bool ContainsCompatibilityName(string name)
        {
            foreach (Compatibility comp in Compatibilities)
            {
                if (comp.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
