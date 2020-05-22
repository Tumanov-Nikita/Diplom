using System;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.IO;
using DIPLOM.Controller;
using DIPLOM.Model;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Windows.Controls;
using DIPLOM.View;

namespace DIPLOM.Infrastructure
{
    public class Parsing
    {
        static DB_Context DB;
        string currPath = AppDomain.CurrentDomain.BaseDirectory;

        public Parsing(DB_Context db)
        {
            DB = db;
        }


        public void Parse()
        {
            string link = @"https://parts.uaz.ru/price_list.php";
            const string FileName = "PriceList.xls";
            string filePath = Path.Combine(currPath, FileName);
            if (!File.Exists(filePath)) {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(new Uri(link), FileName);
                    webClient.Dispose();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Произошла ошибка скачивания данных\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            ProgressBarWindow progressBarWindow = new ProgressBarWindow(DB, FileName);
            progressBarWindow.ShowDialog();
        }
       
    }
}
