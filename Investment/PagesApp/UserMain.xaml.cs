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
using Investment.ADOApp;

namespace Investment.PagesApp
{
    public partial class UserMain : Page
    {
        public User UserCurrent { get; set; }

        public UserMain()
        {
            InitializeComponent();
            SetData();

            listTemplate.ItemsSource = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser).ToList();
        }

        public UserMain(User user)
        {
            InitializeComponent();
            UserCurrent = user;
            SetData();

          


            Thread thread = new Thread(SetLoopData) { IsBackground = true };
            thread.Start();
        
        }

        public void SetLoopData()
        {

            while(true)
            {
                List<StockMarket> stockMarkets = App.Connection.StockMarket.ToList();
                List<Stock> listStocks = App.Connection.Stock.ToList();




                List<BrokerageAccount> listStock = new List<BrokerageAccount>();
                var listStockDB = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser).ToList();

           


                foreach (var item in listStockDB)
                {
                    if (item.Count != 0)
                    {
                        listStock.Add(item);
                    }
                }
               

                Dispatcher.Invoke(() => listTemplate.ItemsSource = listStock);





                Thread.Sleep(10000);

                //foreach (var stock in listStocks)
                //{
                
                //    var lastStock = App.Connection.StockMarket.Where(x => x.IdStock == stock.IdStock).ToList().LastOrDefault();
                //    Thread.Sleep(10000);
                //}
            }
            
        }

        private void BtnStocks_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StockMarketPage(UserCurrent));
        }

        public void SetData()
        {
            TxtBalance.Text = UserCurrent.Balance.ToString();

            var brokerage = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser).ToList();

            if(brokerage != null)
            {
                int sumStocks = 0;

                foreach (var stock in brokerage)
                {
                    sumStocks += (int)(stock.Stock.Price * stock.Count);
                }

                TxtBank.Text = (UserCurrent.Balance + sumStocks).ToString();
            } else
            {
                TxtBank.Text = UserCurrent.Balance.ToString();
            }
        }

        private void listTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BrokerageAccount brokerAcc = listTemplate.SelectedItem as BrokerageAccount;

            try
            {
                StockPerson stockPerson = new StockPerson(brokerAcc.Stock, UserCurrent);
                stockPerson.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
