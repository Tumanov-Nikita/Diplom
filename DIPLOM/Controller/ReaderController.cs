using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace DIPLOM.Controller
{
    class ReaderController
    {
        static DB_Context DB;
        private AutoPartController autoPartCtrl;
        private GroupController groupCtrl;
        private string[] whiteList = { "ГАЗ", "УАЗ", "ЗМЗ", "ПАЗ", "СГР" };

        public ReaderController(DB_Context db)
        {
            DB = db;
            ClearAutoParts();
            autoPartCtrl = new AutoPartController(DB);
            groupCtrl = new GroupController(DB);
            
        }

        public void Add(string article, string name, string groupName, string subGroupName,
                        double price, string proportions, double amount, double weight)
        {
            try
            {
                List<Compatibility> compatibilities = new List<Compatibility>();    
                if (name.Contains("(ДЛЯ"))
                {
                    List<string> compatibilityNames = Ownership(name);
                    if (compatibilityNames != null) {
                        foreach (string str in compatibilityNames) {
                            if (str == null)
                            {
                                return;
                            }
                            //Compatibility findCompatibility = (Compatibility)DB.Compatibilities.Where(c => c.Name == str).FirstOrDefault();
                            //if (findCompatibility == null)
                            //{
                                Compatibility newComp = new Compatibility(str);
                                DB.Compatibilities.Add(newComp);
                                compatibilities.Add(newComp);
                            //}
                            //else
                            //{
                            //    compatibilities.Add(findCompatibility);
                            //}
                            DB.SaveChanges();
                        }
                    }
                }
                else
                {
                    //compatibilities.Add(DB.Compatibilities.Where(c => c.Name == "ОБЩЕЕ").FirstOrDefault());
                    compatibilities.Add(new Compatibility("ОБЩЕЕ"));
                }

                AutoPart newRow = new AutoPart(article, name, groupName, subGroupName,
                                               price, proportions, amount, weight);
                DB.AutoParts.Add(newRow);
                groupCtrl.Add(groupName, subGroupName);
                foreach (Compatibility comp in compatibilities)
                {
                    newRow.AddCompatibility(comp);
                }
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка добавления данных в базу\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ClearAutoParts()
        {
            DB.Groups.RemoveRange(DB.Groups);
            DB.Compatibilities.RemoveRange(DB.Compatibilities);
            DB.AutoParts.RemoveRange(DB.AutoParts);
            DB.SaveChanges();
        }

        private List<string> Ownership(string name)
        {

            int startStr = name.IndexOf("(ДЛЯ");
            string substr = name.Substring(startStr);
            int endStr = substr.IndexOf(')');
            if (endStr == -1 || startStr == -1)
            {
                return null;
            }
            string ownerships = substr.Substring(0, endStr);
            ownerships = ownerships.Substring(9, ownerships.Length - 9);
            List<string> listOwners = ownerships.Split(',').ToList();
            
            List<string> result = new List<string>();
            foreach(string word in listOwners)
            {
                if (whiteList.Any(c=> word.Contains(c)))
                {
                    result.Add(word.TrimStart().TrimEnd());
                }
            }
            return result;
        }
    }
}
