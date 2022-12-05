using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Investment.ADOApp;
using Investment.PagesApp;
using Investment.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace Investment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      

        private static Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

 

            MainFrame.NavigationService.Navigate(new SignIn());

            Thread thread = new Thread(Next) { IsBackground = true };
            thread.Start();
        }

        public void Next()
        {
            while (true)
            {
                StockExchange.StockMarketAnalytics(random);
                Thread.Sleep(10000);
            }
        }
    }
}
