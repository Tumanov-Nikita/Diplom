using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Linq;
using System.Windows;

namespace DIPLOM.Controller
{
    class GroupController
    {
        static DB_Context DB;
        public GroupController(DB_Context db)
        {
            DB = db;
        }

        public void Add(string name, string subGroup)
        {
            if (Checkers.Firmness(name))
            {
                try
                {
                    Group newRow = new Group(name);
                    Group equalRecords = (Group)DB.Groups.Where(l => l.Name.Equals(name)).FirstOrDefault();
                    if (equalRecords != null)
                    {
                        equalRecords.AddSubGroup(subGroup);
                    }
                    else
                    {
                        newRow.AddSubGroup(subGroup);
                        DB.Groups.Add(newRow);
                        DB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка добавления группы\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
