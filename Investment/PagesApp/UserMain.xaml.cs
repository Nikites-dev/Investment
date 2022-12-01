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

            List<BrokerageAccount> listStock = new List<BrokerageAccount>();
            var listStockDB = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser).ToList();

            foreach (var item in listStockDB)
            {
                if(item.Count != 0)
                {
                    listStock.Add(item);
                }
            }
            listTemplate.ItemsSource = listStock;
        
        }

        private void BtnStocks_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StockMarket(UserCurrent));
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

            StockPerson stockPerson = new StockPerson(brokerAcc.Stock, UserCurrent);
            stockPerson.Show();
        }
    }
}
