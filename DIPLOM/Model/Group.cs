using System.Collections.Generic;

namespace DIPLOM.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<SubGroup> SubGroups { get; set; }
        public bool Selected { get; set; }
        }
}