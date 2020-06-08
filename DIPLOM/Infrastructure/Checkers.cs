using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DIPLOM.Infrastructure
{
    public static class Checkers
    {
        public static bool Firmness(string str)
        {
            return str == "" ? false : true;
        }

        public static bool Firmness(double? d)
        {
            return d == null ? false : true;
        }

        public static string UnPointReplace(string text)
        {
            char c = '.';
            if (text.Contains(c.ToString()))
            {
                text = text.Replace('.', ',');
            }
            return text;
        }

        public static bool PriceValidation(string price)
        {
            if (price == "")
            {
                return true;
            }
            else if (!Regex.IsMatch(price, @"^\d{1,7}(\.||\,)?[0-9]{0,2}$"))
            {
                MessageBox.Show("Цена должна содержать не более 7 цифр до запятой и не более 2 после", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static bool WeightValidation(string weight)
        {
            if (weight == "")
            {
                return true;
            }
            else if (!Regex.IsMatch(weight, @"^\d{1,5}(\.||\,)?[0-9]{0,3}$"))
            {
                MessageBox.Show("Масса должна содержать не более 5 цифр до запятой и не более 3 после", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static bool CapacityValidation(string capacity)
        {
            if (capacity == "")
            {
                return true;
            }
            else if (!Regex.IsMatch(capacity, @"^\d{1,5}(\.||\,)?[0-9]{0,3}$"))
            {
                MessageBox.Show("Объем должен содержать не более 5 цифр до запятой и не более 3 после", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static bool AmountValidation(string amount)
        {
            if (Regex.Match(amount, @"\D") != null)
            {
                MessageBox.Show("В поле \"Количество\" допустимы только числа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static double CapacityCalc(string capacity)
        {
            string[] mesaures = capacity.Split('*');
            double resultCapacity = 0;
            foreach(string m in mesaures)
            {
                resultCapacity += Convert.ToDouble(UnPointReplace(m));
            }
            return resultCapacity;
        }

        public static bool ParametersValidation(string price, string weight, string capacity)
        {
            if (price == "" && weight == "" && capacity == "" && price == "0" && weight == "0" && capacity == "0")
            {
                return false;
            }
            return true;
        }

        public static bool CheckSelectedGroups(ObservableCollection<GroupView> groupList)
        {
            var check = groupList.Where(g => g._isChecked).FirstOrDefault();
            if (check == null)
            {
                MessageBox.Show("Выберите хотя бы одну обязательную группу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
} 
