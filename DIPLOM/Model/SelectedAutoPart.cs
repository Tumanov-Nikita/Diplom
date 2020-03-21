using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class SelectedAutoPart
    {
        public int Id { get; set; }
        public AutoPart AutoPart;
        public List<Car> CompatibilityCars { get; set; }
        public double Amount;
        public UnitOfMeasure Measure;
    }
}
