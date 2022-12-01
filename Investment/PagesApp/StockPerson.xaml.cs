using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;
using Investment.ADOApp;
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

namespace Investment.PagesApp
{
    public partial class StockPerson : Window
    {
        public Stock StockCurrent { get; set; }
        public User UserCurrent { get; set; }

        public StockPerson()
        {
            InitializeComponent();
        }

        public StockPerson(Stock stock, User user)
        {
            InitializeComponent();
            StockCurrent = stock;
            UserCurrent = user;

            SetData();
        }

        public void SetData()
        {
            TxtName.Text = StockCurrent.Company.Name;
            TxtShortName.Text = StockCurrent.Company.ShortName;
            TxtSector.Text = StockCurrent.Company.Sector.NameSector;
            TxtPrice.Text = StockCurrent.Price.ToString();
            TxtInfo.Text = StockCurrent.Company.Info;

            MemoryStream stream = new MemoryStream(StockCurrent.Company.ImageLogo);
            ImageLogo.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            BrokerageAccount brokerage = null;

            try
            {
                brokerage = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser && x.IdStock == StockCurrent.IdStock).FirstOrDefault();
            } catch (Exception ex)
            {
                
            }
           

            if(brokerage == null)
            {
                TxtCountInPortfel.Text = "0 шт.";
                TxtPriceInPortfel.Text = "0 р.";
                TxtProcentInPortfel.Text = "0 ";
            }else
            {
                int sumStocks = (int)(brokerage.Stock.Price * brokerage.Count);
        
                TxtCountInPortfel.Text = $"{brokerage.Count} шт.";
                TxtPriceInPortfel.Text = $"{sumStocks} р.";
                TxtProcentInPortfel.Text = " ";
            }
        }



        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Buy(object sender, RoutedEventArgs e)
        {
            int n;
            bool isNumeric = int.TryParse(EdAmount.Text, out n);

            if (isNumeric)
            {
                var brokerage = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser && x.IdStock == StockCurrent.IdStock).FirstOrDefault();
          
                if(int.Parse(EdAmount.Text) * StockCurrent.Price <= UserCurrent.Balance)
                {
                    UserCurrent.Balance -= int.Parse(EdAmount.Text) * StockCurrent.Price;

                    if (brokerage == null)
                    {
                        BrokerageAccount newBrokerAcc = new BrokerageAccount();
                        newBrokerAcc.IdUser = UserCurrent.IdUser;
                        newBrokerAcc.IdStock = StockCurrent.IdStock;
                        newBrokerAcc.Count = int.Parse(EdAmount.Text);

                        App.Connection.BrokerageAccount.Add(newBrokerAcc);
                    }
                    else
                    {
                        brokerage.Count += int.Parse(EdAmount.Text);
                    }
                    App.Connection.SaveChanges();
                    SetData();
                    MessageBox.Show("SUCCES!");
                } 
                else
                {
                    MessageBox.Show("Недостаточно средств!");
                }

            } else
            {
                MessageBox.Show("НЕ Число");
            }
        }

        private void Button_Sell(object sender, RoutedEventArgs e)
        {
            int n;
            bool isNumeric = int.TryParse(EdAmount.Text, out n);

            if (isNumeric)
            {
                var brokerage = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser && x.IdStock == StockCurrent.IdStock).FirstOrDefault();

              

                if (brokerage != null)
                {
                    if (int.Parse(EdAmount.Text) <= brokerage.Count)
                    {
                        UserCurrent.Balance += int.Parse(EdAmount.Text) * StockCurrent.Price;

                        brokerage.Count -= int.Parse(EdAmount.Text);

                        MessageBox.Show("SUCCES!");
                    }
                }
                else
                {
                    brokerage.Count += int.Parse(EdAmount.Text);
                }
                App.Connection.SaveChanges();
                SetData();
            }
            else
            {
                MessageBox.Show("НЕ Число");
            }
        }
    }
}

