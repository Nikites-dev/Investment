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
    public partial class UserMain : Page
    {
        public User UserCurrent { get; set; }

        public bool IsMyValueNegative { get { return (MargeValue < 0); } }

        public int MargeValue { get; set; }

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


            MargeValue = 10;

            Thread thread = new Thread(SetLoopData) { IsBackground = true };
            thread.Start();
        }

        public void SetLoopData()
        {

            while(true)
            {
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
                try
                {
                    Dispatcher.Invoke(() => listTemplate2.ItemsSource = CalculateProfit());
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
                Dispatcher.Invoke(() => SetData());

                Thread.Sleep(10000);
            }
        }

        public List<Profit> CalculateProfit()
        {
            if (App.Connection != null)
            {
                List<Stock> listStocks = App.Connection.Stock.ToList();
                var listStockDB = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser).ToList();

                List<Profit> listProfits = new List<Profit>();

                foreach (var item in listStockDB)
                {
                    if (item.Count != 0)
                    {
                        Profit profit = new Profit();

                        double margin = (double)(item.Count * item.Stock.Price - item.Amount);
                        profit.Price = (int)margin;
                        profit.Procent = Math.Truncate(Math.Abs((double)(margin * 100 / (item.Amount - margin))) * 100) / 100;
                        profit.Amount = (int)(item.Count * item.Stock.Price);

                        try
                        {
                            profit.ProcentOfCompany = Math.Truncate(Math.Abs((double)(item.Count * 100 / (item.Stock.Company.NumberOfStocks))) * 100) / 100;
                        } catch
                        {
                            profit.ProcentOfCompany = 0;
                        }
                    

                        if (profit.Price < 0)
                        {
                            profit.PlusOrMinus = "-";
                        }
                        else
                        {
                            profit.PlusOrMinus = "+";
                        }

                        listProfits.Add(profit);
                    }
                }
                return listProfits;
            }
            else
            {
                return null;
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

                double marg = (double)(UserCurrent.Balance + sumStocks - 10000);
                double amount = (double)(UserCurrent.Balance + sumStocks);
                TxtProfit.Text = (marg).ToString() + "р. " + Math.Truncate(Math.Abs((double)(marg * 100 / (amount - marg))) * 100) / 100 + "%";

                if(marg < 0)
                {
                    TxtProfit.Foreground = Brushes.Red;
                } else
                {
                    TxtProfit.Foreground = Brushes.Green;
                }

            } else
            {
                TxtBank.Text = UserCurrent.Balance.ToString();
            }
        }

        private void listTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BrokerageAccount brokerAcc = listTemplate.SelectedItem as BrokerageAccount;

            
            if (brokerAcc != null)
            {
                StockPerson stockPerson = new StockPerson(brokerAcc.Stock, UserCurrent);
                stockPerson.Show();
            }
        }
    }
}
