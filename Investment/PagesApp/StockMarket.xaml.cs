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
    public partial class StockMarket : Page
    {
  
        public User UserCurrent { get; set; }

        public StockMarket()
        {
            InitializeComponent();
            listTemplate.ItemsSource = App.Connection.Stock.ToList();
        }

        public StockMarket(User user)
        {
            InitializeComponent();
            listTemplate.ItemsSource = App.Connection.Stock.ToList();
            UserCurrent = user;
        }

        private void listTemplate_Selected(object sender, RoutedEventArgs e)
        {
            Stock stock = listTemplate.SelectedItem as Stock;

            StockPerson stockPerson = new StockPerson(stock, UserCurrent);
            stockPerson.Show();
        }

        private void BtnMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserMain(UserCurrent));
        }
    }
}
