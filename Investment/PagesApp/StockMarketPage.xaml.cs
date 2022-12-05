using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using Investment.ADOApp;
using Investment.Models;

namespace Investment.PagesApp
{
    public partial class StockMarketPage : Page
    {
  
        public User UserCurrent { get; set; }

        public StockMarketPage()
        {
            InitializeComponent();
            listTemplate.ItemsSource = App.Connection.Stock.ToList();
        }

        public StockMarketPage(User user)
        {
            InitializeComponent();
            listTemplate.ItemsSource = App.Connection.Stock.ToList();
            UserCurrent = user;

            Thread thread = new Thread(SetLoopData) { IsBackground = true };
            thread.Start();
        }

        public void SetLoopData()
        {
            List<Profit> listProfits;
            List<Stock> listStocks;
            StockMarket stockMarket;
            while (true)
            {
               listProfits = new List<Profit>();
        
               listStocks = App.Connection.Stock.ToList();

                foreach (Stock stock in listStocks)
                {
                    Profit profit = new Profit();

                    stockMarket = App.Connection.StockMarket.Where(x => x.IdStock == stock.IdStock).FirstOrDefault();
                    double margin = (double)(stock.Price - stockMarket.LastTransaction);

                    profit.Price = (int)(margin);
                    profit.ProcentOfCompany = 0;
                 
                    profit.Procent = Math.Truncate(Math.Abs((double)(margin * 100 / (stock.Price - margin))) * 100) / 100;

                    if (margin < 0)
                    {
                        profit.PlusOrMinus = "-";
                    }
                    else
                    {
                        profit.PlusOrMinus = "+";
                    }
                    listProfits.Add(profit);
                }

              
                
                Dispatcher.Invoke(() => listTemplate.ItemsSource = App.Connection.Stock.ToList());
                Dispatcher.Invoke(() => listTemplate2.ItemsSource = listProfits);

                Thread.Sleep(10000);
            }
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
