using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class Complectation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SelectedAutoPart> SelectedParts { get; set; }
        public double Price { get; set; }
    }
}
