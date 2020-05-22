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
        public List<AutoPart> Compatibility { get; set; } //Как реализовать совместимость?
        public double Price { get; set; }
        public string Proportions { get; set; }
        public double Weight { get; set; }
        public double Amount { get; set; }
        public string Photo { get; set; }


        public AutoPart(string article, string name, string groupName, string subGroupName, 
                        double price, string proportions, double amount, double weight, string photo)
        {
            Article = article;
            Name = name;
            GroupName = groupName;
            SubGroupName = subGroupName;
            Price = price;
            Proportions = proportions;
            Amount = amount;
            Weight = weight;
            Photo = photo;
        }

        public AutoPart()
        {
        }
    }
}
