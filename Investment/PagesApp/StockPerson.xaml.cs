using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;
using Investment.ADOApp;
using Investment.Models;
using Brushes = System.Windows.Media.Brushes;

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

            Thread thread = new Thread(SetLoopData) { IsBackground = true };
            thread.Start();
        }

        public void SetLoopData()
        {
            StockMarket stockMarket;
            while (true)
            {
             
                    stockMarket = App.Connection.StockMarket.Where(x => x.IdStock == StockCurrent.IdStock).FirstOrDefault();
                    double margin = (double)(StockCurrent.Price - stockMarket.LastTransaction);

                var brokerage = App.Connection.BrokerageAccount.Where(x => x.IdUser == UserCurrent.IdUser && x.IdStock == StockCurrent.IdStock).FirstOrDefault();

                double marginPortfel = (double)(brokerage.Count * brokerage.Stock.Price - brokerage.Amount);
                var procentPorfel = Math.Truncate(Math.Abs((double)(marginPortfel * 100 / (brokerage.Amount - marginPortfel))) * 100) / 100;






                // Dispatcher.Invoke(() => TxtProcentInPortfel.Text = marginPortfel + " | " + procentPorfel + "%");
                Dispatcher.Invoke(() => TxtProcentInPortfel.Text = marginPortfel + " р. (" + procentPorfel + "%)");

                Dispatcher.Invoke(() => TxtProcentCommon.Text = (Math.Truncate(Math.Abs((double)(margin * 100 / (StockCurrent.Price - margin))) * 100) / 100).ToString() + "%");


                if (margin < 0) { Dispatcher.Invoke(() => TxtProcentCommon.Foreground = Brushes.Red); }
                else { Dispatcher.Invoke(() => TxtProcentCommon.Foreground = Brushes.Green); }

                if (marginPortfel < 0) { Dispatcher.Invoke(() => TxtProcentInPortfel.Foreground = Brushes.Red); }
                else { Dispatcher.Invoke(() => TxtProcentInPortfel.Foreground = Brushes.Green); }

                Dispatcher.Invoke(() => SetData());

                 Thread.Sleep(10000);
            }
        }

        public void SetData()
        {
            TxtBalance.Text = UserCurrent.Balance.ToString() + " р.";
            TxtName.Text = StockCurrent.Company.Name;
            TxtShortName.Text = StockCurrent.Company.ShortName;
            TxtSector.Text = StockCurrent.Company.Sector.NameSector;
            TxtPrice.Text = StockCurrent.Price.ToString() + " р.";
            TxtInfo.Text = StockCurrent.Company.Info;
            TxtCount.Text = StockCurrent.Company.NumberOfStocks.ToString() + " шт.";

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

                    if (brokerage.Count >= brokerage.Stock.Company.NumberOfStocks)
                    {
                        brokerage.Stock.Company.NumberOfStocks *= 10;
                    }


                    if (brokerage == null)
                    {
                        BrokerageAccount newBrokerAcc = new BrokerageAccount();
                        newBrokerAcc.IdUser = UserCurrent.IdUser;
                        newBrokerAcc.IdStock = StockCurrent.IdStock;
                        newBrokerAcc.Count = int.Parse(EdAmount.Text);
                        newBrokerAcc.Amount = int.Parse(EdAmount.Text) * StockCurrent.Price;
                        App.Connection.BrokerageAccount.Add(newBrokerAcc);
                    }
                    else
                    {
                        brokerage.Count += int.Parse(EdAmount.Text);
                        brokerage.Amount += int.Parse(EdAmount.Text) * StockCurrent.Price;
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
                        brokerage.Amount -= int.Parse(EdAmount.Text) * StockCurrent.Price;

                        MessageBox.Show("SUCCES!");
                    }

                    if(brokerage.Count == 0)
                    {
                        brokerage.Amount = 0;
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

