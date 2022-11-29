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

namespace Investment.PagesApp
{
    /// <summary>
    /// Interaction logic for StockMarket.xaml
    /// </summary>
    public partial class StockMarket : Page
    {
        public StockMarket()
        {
            InitializeComponent();
            listTemplate.ItemsSource = App.Connection.Stock.ToList();
        }
    }
}
