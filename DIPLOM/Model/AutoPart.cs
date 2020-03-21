using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class AutoPart //Тестовый класс
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Producer Producer { get; set; }
        public List<AutoPart> Compatibility { get; set; }
        public double Price { get; set; }
    }
}
