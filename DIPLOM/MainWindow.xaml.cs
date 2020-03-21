using DIPLOM.Infrastructure;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DIPLOM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pageContent = Parsing.LoadPage(@"http://lsreg.ru/?s=c%23");
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pageContent);
            HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//h2/a");
            foreach (HtmlNode link in links)
                Console.WriteLine("{0} - {1}", link.InnerText, link.GetAttributeValue("href", ""));
        }
    }
}
