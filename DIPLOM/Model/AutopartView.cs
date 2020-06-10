using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DIPLOM.Model
{
    public class AutopartView
    {
        public AutoPart AutoPart { get; set; }
        public bool _isChecked { get; set; }

        public AutopartView(AutoPart autoPart, bool isSelected)
        {
            AutoPart = autoPart;
            _isChecked = isSelected;
        }
    }
}
