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
        public Group Group;
        public ComboBox Combo;

        public AutopartView(Group group, ComboBox combo)
        {
            Group = group;
            Combo = combo;
        }
    }
}
