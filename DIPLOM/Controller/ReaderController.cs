using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DIPLOM.Controller
{
    class ReaderController
    {
        static DB_Context DB;

        public ReaderController(DB_Context db)
        {
            DB = db;
            ClearAutoParts();
        }

        public void Add(string article, string name, string groupName, string subGroupName,
                        double price, string proportions, double amount, double weight, string photo)
        {
            try
            {
                AutoPart newRow = new AutoPart(article, name, groupName, subGroupName,
                                               price, proportions, amount, weight, photo);
                    DB.AutoParts.Add(newRow);
                    DB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка добавления данных в базу\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ClearAutoParts()
        {
            DB.AutoParts.RemoveRange(DB.AutoParts);
            DB.SaveChanges();
        }
    }
}
