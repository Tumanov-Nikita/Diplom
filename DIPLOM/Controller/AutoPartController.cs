using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DIPLOM.Controller
{
    class AutoPartController
    {
        static DB_Context DB;
        private GroupController groupCtrl;
        public AutoPartController(DB_Context db)
        {
            DB = db;
            groupCtrl = new GroupController(DB);
        }

        public void Add(string article, string name, string groupName, string subGroupName,
                        double price, string proportions, double amount, double weight, string match)
        {
            if (Checkers.Firmness(article) && Checkers.Firmness(groupName) && 
                Checkers.Firmness(name) && Checkers.Firmness(price))
            {
                try
                {
                    AutoPart newRow = new AutoPart(article, name, groupName, subGroupName,
                                               price, proportions, amount, weight);
                    var equalRecords = DB.AutoParts.Where(l => l.Name.Equals(name));
                    if (equalRecords.Any())
                    {
                        MessageBox.Show("Такая запчасть уже существует в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Compatibility compatibility;
                        if (match == "")
                        {
                            compatibility = (Compatibility)DB.Compatibilities.Where(c => c.Name=="ОБЩЕЕ").FirstOrDefault();
                        }
                        else
                        {
                            compatibility = (Compatibility)DB.Compatibilities.Where(c => c.Name == match).FirstOrDefault();
                        }
                        newRow.AddCompatibility(compatibility);
                        groupCtrl.Add(groupName, subGroupName);
                        DB.AutoParts.Add(newRow);
                        DB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка добавления запчасти\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



    }
}
