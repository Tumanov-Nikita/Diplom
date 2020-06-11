using DIPLOM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DIPLOM.Infrastructure
{
    class SavingReport
    {
        Microsoft.Office.Interop.Excel.Application ExcelApp;
        Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
        Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;

        public SavingReport()
        {
            ExcelApp = new Microsoft.Office.Interop.Excel.Application();
        }

        public void SaveInExcelReport(List<AutoPart> resultParts, string fileName)
        {
            try
            {
                Object missing = Type.Missing;

                ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
                ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

                ExcelWorkSheet.Cells[1, 1] = "Отчет подобранных авто-компонентов";
                ExcelWorkSheet.Cells[2, 1] = String.Format("Дата составления: {0}", DateTime.Now.ToString());

                ExcelWorkSheet.Cells[4, 1] = "№";
                ExcelWorkSheet.Cells[4, 2] = "Артикул";
                ExcelWorkSheet.Cells[4, 3] = "Наименование";
                ExcelWorkSheet.Cells[4, 4] = "Группа";
                ExcelWorkSheet.Cells[4, 5] = "Стоимость";
                ExcelWorkSheet.Cells[4, 6] = "Вес";
                ExcelWorkSheet.Cells[4, 7] = "Объем";
                ExcelWorkSheet.Cells[4, 8] = "Размеры";

                (ExcelWorkSheet.Cells[1, 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[2, 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 2] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 3] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 4] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 5] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 6] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 7] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                (ExcelWorkSheet.Cells[4, 8] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;

                (ExcelWorkSheet.Cells[4, 1] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 2] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 3] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 4] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 5] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 6] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 7] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[4, 8] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                CircleCell(4, 1);
                CircleCell(4, 2);
                CircleCell(4, 3);
                CircleCell(4, 4);
                CircleCell(4, 5);
                CircleCell(4, 6);
                CircleCell(4, 7);
                CircleCell(4, 8);


                double SumPrice = 0;
                double SumWeight = 0;
                double SumCapacity = 0;

                for (int i = 0; i < resultParts.Count; i++)
                {
                    SumPrice += resultParts[i].Price;
                    SumWeight += resultParts[i].Weight;
                    SumCapacity += Checkers.CapacityCalc(resultParts[i].Proportions);
                    ExcelApp.Cells[i + 5, 1] = i+1;
                    CircleCell(i + 5, 1);
                    ExcelApp.Cells[i + 5, 2] = resultParts[i].Article;
                    CircleCell(i + 5, 2);
                    ExcelApp.Cells[i + 5, 3] = resultParts[i].Name;
                    CircleCell(i + 5, 3);
                    ExcelApp.Cells[i + 5, 4] = resultParts[i].GroupName;
                    CircleCell(i + 5, 4);
                    ExcelApp.Cells[i + 5, 5] = resultParts[i].Price;
                    CircleCell(i + 5, 5);
                    ExcelApp.Cells[i + 5, 6] = resultParts[i].Weight;
                    CircleCell(i + 5, 6);
                    ExcelApp.Cells[i + 5, 7] = Checkers.CapacityCalc(resultParts[i].Proportions);
                    CircleCell(i + 5, 7);
                    ExcelApp.Cells[i + 5, 8] = resultParts[i].Proportions;
                    CircleCell(i + 5, 8);
                }

                ExcelWorkSheet.Cells[resultParts.Count + 5, 1] = "ИТОГО:";
                (ExcelWorkSheet.Cells[resultParts.Count + 5, 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                ExcelWorkSheet.Cells[resultParts.Count + 5, 5] = SumPrice.ToString();
                (ExcelWorkSheet.Cells[resultParts.Count + 5, 5] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                ExcelWorkSheet.Cells[resultParts.Count + 5, 6] = SumWeight.ToString();
                (ExcelWorkSheet.Cells[resultParts.Count + 5, 6] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
                ExcelWorkSheet.Cells[resultParts.Count + 5, 7] = SumCapacity.ToString();
                (ExcelWorkSheet.Cells[resultParts.Count + 5, 7] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;

                (ExcelWorkSheet.Cells[1, 1] as Microsoft.Office.Interop.Excel.Range).ColumnWidth = 7;
                (ExcelWorkSheet.Cells[1, 2] as Microsoft.Office.Interop.Excel.Range).ColumnWidth = 20;
                (ExcelWorkSheet.Cells[1, 3] as Microsoft.Office.Interop.Excel.Range).ColumnWidth = 70;
                (ExcelWorkSheet.Cells[1, 4] as Microsoft.Office.Interop.Excel.Range).EntireColumn.AutoFit();
                (ExcelWorkSheet.Cells[1, 5] as Microsoft.Office.Interop.Excel.Range).EntireColumn.AutoFit();
                (ExcelWorkSheet.Cells[1, 6] as Microsoft.Office.Interop.Excel.Range).EntireColumn.AutoFit();
                (ExcelWorkSheet.Cells[1, 7] as Microsoft.Office.Interop.Excel.Range).EntireColumn.AutoFit();
                (ExcelWorkSheet.Cells[1, 8] as Microsoft.Office.Interop.Excel.Range).EntireColumn.AutoFit();


               

                (ExcelWorkSheet.Cells[5, 1] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                (ExcelWorkSheet.Cells[5, 1] as Microsoft.Office.Interop.Excel.Range).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;


                ExcelWorkBook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ExcelWorkBook.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка выгрузки отчета\n"+ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                int hWnd = ExcelApp.Application.Hwnd;
                CloseExcel.TryKillProcessByMainWindowHwnd(hWnd);
            }
        }

        private void CircleCell(int y, int x)
        {
            (ExcelWorkSheet.Cells[y, x] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            (ExcelWorkSheet.Cells[y, x] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            (ExcelWorkSheet.Cells[y, x] as Microsoft.Office.Interop.Excel.Range).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        }
    }
}
