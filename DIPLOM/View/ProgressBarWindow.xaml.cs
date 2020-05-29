using DIPLOM.Controller;
using DIPLOM.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        { 
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            Reading(worker);

            if (worker.CancellationPending == true)
            {
                MessageBox.Show("Обновление прервано пользователем", "", MessageBoxButton.OK, MessageBoxImage.Information);
                e.Result = false;
            }
            else
            {
                e.Result = true;
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
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
            else if ((bool)e.Result == true)
            {
                MessageBox.Show("Загрузка завершена", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Close();
        }

        private void Reading(System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            Application ObjExcel = new Application();

            string filePath = System.IO.Path.Combine(currPath, FilePath);
            string[] stopList = { "SALE", "ПОЛИГРАФИЯ" };
            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(filePath, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            int iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[XlDirection.xlUp].Row;
            Range ObjRange = ObjWorkSheet.Range["A5:I" + iLastRow];
            var arrData = (object[,])ObjRange.Value;
            int length = arrData.GetLength(0);

            for (int i = 1; i < length; i++)
            {
                if (!worker.CancellationPending)
                {
                    try
                    {
                        string article = arrData[i, 1].ToString(),
                    name = arrData[i, 2].ToString(),
                    groupName = arrData[i, 3].ToString(),
                    subGroupName = arrData[i, 4].ToString(),
                    proportions = arrData[i, 7].ToString();
                        double price = Convert.ToDouble(UnPointReplace(arrData[i, 6].ToString())),
                        amount = AmountParse(arrData[i, 8].ToString()),
                        weight = Convert.ToDouble(UnPointReplace(arrData[i, 9].ToString()));
                        if (stopList.Any(c => groupName.Contains(c)))
                        {
                            continue;
                        }
                        else
                        {
                            ReadContr.Add(article, name, groupName, subGroupName, price, proportions, amount, weight);
                            backgroundWorker.ReportProgress((i * 1000 / length) + 1);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                else
                {
                    backgroundWorker.CancelAsync();
                }
            }

            int hWnd = ObjExcel.Application.Hwnd;
            TryKillProcessByMainWindowHwnd(hWnd);
        }

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                Process.GetProcessById((int)processID).Kill();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }
    }
}
