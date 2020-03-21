using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PassHash { get; set; }
        public Role Role { get; set; }
    }
}
