using DIPLOM.Controller;
using DIPLOM.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace DIPLOM.View
{
    /// <summary>
    /// Логика взаимодействия для ProgressBar.xaml
    /// </summary>
    public partial class ProgressBarWindow : System.Windows.Window
    {
        DB_Context DB;
        string FilePath;
        string currPath = AppDomain.CurrentDomain.BaseDirectory;
        ReaderController ReadContr;
        BackgroundWorker worker;

        public ProgressBarWindow(DB_Context db, string filepath)
        {
            InitializeComponent();
            DB = db;
            FilePath = filepath;
            ReadContr = new ReaderController(DB);
            worker = new BackgroundWorker();
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        { 
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Reading(worker);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        void worker_CancellationPending()
        {
            Console.WriteLine("gtyhuikijuhgf");
        }

        private double AmountParse(string strAmount)
        {
            if (strAmount != "Нет в наличии")
            {
                return Convert.ToDouble(strAmount.TrimStart('>', '<'));
            }
            else
            {
                return 0;
            }
        }

        private string UnPointReplace(string text)
        {
            char c = '.';
            if (text.Contains(c.ToString()))
            {
                text = text.Replace('.', ',');
            }
            return text;
        }


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }
            MessageBox.Show("Загрузка завершена", "",MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Reading(System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            Application ObjExcel = new Application();

            string filePath = System.IO.Path.Combine(currPath, FilePath);

            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(filePath, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            int iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[XlDirection.xlUp].Row;
            var arrData = (object[,])ObjWorkSheet.Range["A5:J" + iLastRow].Value;
            int length = arrData.GetLength(0);

            for (int i = 1; i < length; i++)
            {
                if (!backgroundWorker.CancellationPending)
                {
                    try
                    {
                        string article = arrData[i, 1].ToString(),
                    name = arrData[i, 2].ToString(),
                    groupName = arrData[i, 3].ToString(),
                    subGroupName = arrData[i, 4].ToString(),
                    proportions = arrData[i, 7].ToString(),
                    photo = arrData[i, 10].ToString();
                        double price = Convert.ToDouble(UnPointReplace(arrData[i, 6].ToString())),
                        amount = AmountParse(arrData[i, 8].ToString()),
                        weight = Convert.ToDouble(UnPointReplace(arrData[i, 9].ToString()));
                        if (groupName.Contains("SALE"))
                        {
                            continue;
                        }
                        else
                        {
                            ReadContr.Add(article, name, groupName, subGroupName, price, proportions, amount, weight, photo);
                            backgroundWorker.ReportProgress(i * 100 / length);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                else
                {
                    backgroundWorker.CancelAsync();
                }
            }
            ObjWorkBook.Close(false);
            ObjExcel.Quit();
        }
    }
}
