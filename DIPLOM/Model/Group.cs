using System.Collections.Generic;

namespace DIPLOM.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<string> SubGroups { get; set; }

        public Group(string name)
        {
            Name = name;
            SubGroups = new List<string>();
        }

        public Group()
        {
        }

        public void AddSubGroup(string SubGroup)
        {
            if (!SubGroups.Contains(SubGroup))
            {
                SubGroups.Add(SubGroup);
            }
        }
    }
}