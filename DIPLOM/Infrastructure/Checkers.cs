using System;
using System.Collections.Generic;
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


        public static bool PriceValidation(string price)
        {
            if (!Regex.IsMatch(price, @"^\d{1,5}(\.||\,)?[0-9]{0,2}$"))
            {
                MessageBox.Show("Цена должна содержать не более 5 цифр до запятой и не более 2 после", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

    }
} 
